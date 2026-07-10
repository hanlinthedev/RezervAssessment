using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rezerv.WhatsApp.Api.Contracts;
using Rezerv.WhatsApp.Application.DTO.Connection;
using Rezerv.WhatsApp.Application.Services.Connection;

namespace Rezerv.WhatsApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConnectionsController : ApiControllerBase
    {
        private readonly ILocationConnectionService _locationConnectionService;

        public ConnectionsController(ILocationConnectionService locationConnectionService)
        {
            _locationConnectionService = locationConnectionService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ConnectionResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ConnectionRequest request, CancellationToken cancellationToken)
        {
            var result = await _locationConnectionService.CreateAsync(
                request,
                cancellationToken);

            if (result.IsFailure)
                return ToErrorResponse(result);

            return CreatedAtAction(
                nameof(Create),
                new { locationId = result.Value!.LocationId },
                result.Value);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ConnectionResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
        {
            var result = await _locationConnectionService.GetAllAsync(cancellationToken);

            return OkResponse(
                result.Value!,
                "Connections retrieved successfully.");
        }

        [HttpGet("{locationId}")]
        [ProducesResponseType(typeof(ApiResponse<ConnectionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByLocationId(
       string locationId,
       CancellationToken cancellationToken)
        {
            var result = await _locationConnectionService.GetByLocationIdAsync(
                locationId,
                cancellationToken);

            if (result.IsFailure)
            {
                return ToErrorResponse(result);
            }

            return OkResponse(
                result.Value!,
                "Connection retrieved successfully.");
        }

        [HttpDelete("{locationId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Disconnect(
            string locationId,
            CancellationToken cancellationToken)
        {
            var result = await _locationConnectionService.DisconnectAsync(
                locationId,
                cancellationToken);

            if (result.IsFailure)
            {
                return ToErrorResponse(result);
            }

            return NoContentResponse();
        }

        [HttpPost("{locationId}/reconnect")]
        [ProducesResponseType(typeof(ApiResponse<ConnectionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Reconnect(
            string locationId,
            CancellationToken cancellationToken)
        {
            var result = await _locationConnectionService.ReconnectAsync(
                locationId,
                cancellationToken);

            if (result.IsFailure)
            {
                return ToErrorResponse(result);
            }

            return OkResponse(
                result.Value!,
                "Connection reconnected successfully.");
        }
    }
}