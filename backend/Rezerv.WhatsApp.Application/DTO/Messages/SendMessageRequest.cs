using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rezerv.WhatsApp.Application.DTO.Messages
{
    public class SendMessageRequest
    {
        [Required]
        public string LocationId { get; init; } = default!;

        [Required]
        [RegularExpression(@"^\+[1-9]\d{7,14}$", ErrorMessage = "Recipient phone must be in E.164 format.")]
        public string RecipientPhone { get; init; } = default!;

        [Required]
        [MinLength(1)]
        public string MessageContent { get; init; } = default!;
    }
}
