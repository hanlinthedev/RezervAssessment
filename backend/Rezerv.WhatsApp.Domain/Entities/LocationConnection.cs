using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Domain.Enums;

namespace Rezerv.WhatsApp.Domain.Entities
{
   public class LocationConnection
   {
      public string Id { get; set; } = default!;
      public string LocationId { get; set; } = default!;
      public string PhoneNumber { get; set; } = default!;
      public string DisplayName { get; set; } = default!;
      public ConnectionState ConnectionState { get; set; }
      public DateTime LastActivityAt { get; set; }
      public DateTime ConnectedAt { get; set; }
      public DateTime UpdatedAt { get; set; }
   }
}