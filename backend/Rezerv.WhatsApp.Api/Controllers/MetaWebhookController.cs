using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rezerv.WhatsApp.Api.Contracts;
using Rezerv.WhatsApp.Application.DTO.Webhook;
using Rezerv.WhatsApp.Application.Services.MetaWebhook;
using Rezerv.WhatsApp.Infrastructure.Meta;

namespace Rezerv.WhatsApp.Api.Controllers
{
    [ApiController]
    [Route("webhook/meta")]
    public class MetaWebhookController : ApiControllerBase
    {
        private readonly IMetaWebhookService _metaWebhookService;
        private readonly MetaWebhookSignatureValidator _signatureValidator;
        private readonly MetaWebhookSettings _settings;

        public MetaWebhookController(
            IMetaWebhookService metaWebhookService,
            MetaWebhookSignatureValidator signatureValidator,
            IOptions<MetaWebhookSettings> settings)
        {
            _metaWebhookService = metaWebhookService;
            _signatureValidator = signatureValidator;
            _settings = settings.Value;
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Verify(
            [FromQuery(Name = "hub.mode")] string? mode,
            [FromQuery(Name = "hub.verify_token")] string? verifyToken,
            [FromQuery(Name = "hub.challenge")] string? challenge)
        {
            if (mode == "subscribe" &&
                verifyToken == _settings.VerifyToken &&
                !string.IsNullOrWhiteSpace(challenge))
            {
                return Content(challenge, "text/plain");
            }

            return StatusCode(StatusCodes.Status403Forbidden, ApiErrorResponse.From("Invalid webhook verification token."));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Receive(CancellationToken cancellationToken)
        {
            Request.EnableBuffering();

            using var reader = new StreamReader(
                Request.Body,
                leaveOpen: true);

            var rawBody = await reader.ReadToEndAsync(cancellationToken);

            Request.Body.Position = 0;

            var signature = Request.Headers["X-Hub-Signature-256"].FirstOrDefault();

            if (!_signatureValidator.IsValid(signature, rawBody))
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            MetaWebhookPayload? payload;

            try
            {
                payload = JsonSerializer.Deserialize<MetaWebhookPayload>(
                    rawBody,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch
            {
                return BadRequest(ApiErrorResponse.From("Invalid webhook payload."));
            }

            if (payload is null)
            {
                return BadRequest(ApiErrorResponse.From("Invalid webhook payload."));
            }

            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(TimeSpan.FromSeconds(4));

            await _metaWebhookService.ProcessAsync(payload, timeoutCts.Token);

            return OkResponse(
                "processed",
                "Webhook processed successfully.");
        }
    }
}