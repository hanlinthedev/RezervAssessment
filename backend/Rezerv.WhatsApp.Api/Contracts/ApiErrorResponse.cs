using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezerv.WhatsApp.Api.Contracts
{
    public class ApiErrorResponse
    {
        public bool Success { get; init; } = false;
        public string Message { get; init; } = default!;

        public static ApiErrorResponse From(string? message) => new()
        {
            Message = message ?? "An unexpected error occurred."
        };
    }
}