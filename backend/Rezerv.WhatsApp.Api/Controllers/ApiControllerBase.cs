using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rezerv.WhatsApp.Api.Contracts;
using Rezerv.WhatsApp.Application.Common;

namespace Rezerv.WhatsApp.Api.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult OkResponse<T>(T data, string? message = null)
        {
            return Ok(ApiResponse<T>.Ok(data, message));
        }

        protected IActionResult CreatedResponse<T>(
            string actionName,
            object routeValues,
            T data,
            string? message = null)
        {
            return CreatedAtAction(
                actionName,
                routeValues,
                ApiResponse<T>.Ok(data, message));
        }

        protected IActionResult NoContentResponse()
        {
            return NoContent();
        }

        protected IActionResult ToErrorResponse(Result result)
        {
            var response = ApiErrorResponse.From(result.Error);

            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(response),
                ErrorType.Conflict => Conflict(response),
                ErrorType.Validation => BadRequest(response),
                ErrorType.BusinessRule => BadRequest(response),
                _ => BadRequest(response)
            };
        }
    }
}