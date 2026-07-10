using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezerv.WhatsApp.Domain.Enums
{
   public enum ConnectionState
   {
      Active,
      Stale,
      Expired,
      Disconnected
   }
}