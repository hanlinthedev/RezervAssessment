using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Domain.Enums;

namespace Rezerv.WhatsApp.Application.DTO.Messages
{
    public class MessageLogResponse
    {
        public string Id { get; init; } = default!;
        public string LocationId { get; init; } = default!;
        public string RecipientPhone { get; init; } = default!;
        public string MessageContent { get; init; } = default!;
        public string? MetaMessageId { get; init; }
        public MessageStatus Status { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? SentAt { get; init; }
        public DateTime? DeliveredAt { get; init; }
        public DateTime? ReadAt { get; init; }
        public DateTime? FailedAt { get; init; }
        public string? FailureReason { get; init; }
    }
}