using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rezerv.WhatsApp.Api.Contracts;
using Rezerv.WhatsApp.Application.DTO.Messages;
using Rezerv.WhatsApp.Application.Services.Message;
using Rezerv.WhatsApp.Domain.Enums;

namespace Rezerv.WhatsApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ApiControllerBase
    {
        private readonly IMessageLogService _messageService;

        public MessagesController(IMessageLogService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("send")]
        [ProducesResponseType(typeof(ApiResponse<MessageLogResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Send(
            [FromBody] SendMessageRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _messageService.SendAsync(
                request,
                cancellationToken);

            if (result.IsFailure)
            {
                return ToErrorResponse(result);
            }

            return OkResponse(
                result.Value!,
                "Message processed successfully.");
        }

        [HttpGet("{locationId}")]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<MessageLogResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByLocationId(
            string locationId,
            [FromQuery] MessageStatus? status,
            CancellationToken cancellationToken)
        {
            var result = await _messageService.GetByLocationIdAsync(
                locationId,
                status,
                cancellationToken);

            return OkResponse(
                result.Value!,
                "Message logs retrieved successfully.");
        }
    }
}