using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rezerv.WhatsApp.Application.DTO.Connection
{
    public class ConnectionRequest
    {
        [Required]
        public string LocationId { get; init; } = default!;
        [Required]
        [RegularExpression(@"^\+[1-9]\d{7,14}$", ErrorMessage = "Phone number must be in E.164 format.")]
        public string PhoneNumber { get; init; } = default!;
        [Required]
        public string DisplayName { get; init; } = default!;
        public DateTime? LastActivityAt { get; init; }
    }
}