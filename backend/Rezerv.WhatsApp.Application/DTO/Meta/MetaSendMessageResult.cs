using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezerv.WhatsApp.Application.DTO.Meta
{
    public record MetaSendMessageResult
    {
        public bool IsSuccess { get; init; }
        public string? MetaMessageId { get; init; }
        public string? ErrorMessage { get; init; }
        public static MetaSendMessageResult Success(string metaMessageId) => new()
        {
            IsSuccess = true,
            MetaMessageId = metaMessageId
        };
        public static MetaSendMessageResult Failure(string errorMessage) => new()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}