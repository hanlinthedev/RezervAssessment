using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezerv.WhatsApp.Domain.Enums
{
   public enum MessageStatus
   {
      Queued,
      Sent,
      Delivered,
      Read,
      Failed
   }
}