using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Domain.Enums;

namespace Rezerv.WhatsApp.Domain.Entities
{
   public class MessageLog
   {
      public string Id { get; set; } = default!;
      public string LocationId { get; set; } = default!;
      public string RecipientPhone { get; set; } = default!;
      public string MessageContent { get; set; } = default!;
      public string? MetaMessageId { get; set; }
      public MessageStatus Status { get; set; }
      public DateTime CreatedAt { get; set; }
      public DateTime? SentAt { get; set; }
      public DateTime? DeliveredAt { get; set; }
      public DateTime? ReadAt { get; set; }
      public DateTime? FailedAt { get; set; }
      public string? FailureReason { get; set; }
   }
}
