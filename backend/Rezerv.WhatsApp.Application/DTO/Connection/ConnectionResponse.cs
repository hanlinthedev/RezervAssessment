using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Domain.Enums;

namespace Rezerv.WhatsApp.Application.DTO.Connection
{
    public class ConnectionResponse
    {
        public string Id { get; init; } = default!;
        public string LocationId { get; init; } = default!;
        public string PhoneNumber { get; init; } = default!;
        public string DisplayName { get; init; } = default!;
        public ConnectionState ConnectionState { get; init; }
        public DateTime LastActivityAt { get; init; }
        public DateTime ConnectedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
    }
}