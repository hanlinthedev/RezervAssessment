using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Domain.Enums;

namespace Rezerv.WhatsApp.Domain.Policies
{
   public class MessageStatusLifeCyclePolicy
   {
      private static readonly Dictionary<MessageStatus, int> Rule = new Dictionary<MessageStatus, int>
   {
      { MessageStatus.Queued, 0 },
      { MessageStatus.Sent, 1 },
      { MessageStatus.Delivered, 2 },
      { MessageStatus.Read, 3 },
      { MessageStatus.Failed, 4 }
   };
      public static bool ShouldApplyTransition(MessageStatus currentStatus, MessageStatus nextStatus)
      {
         if (currentStatus == nextStatus)
         {
            return false;
         }
         if (currentStatus == MessageStatus.Failed)
         {
            return false;
         }
         if (currentStatus == MessageStatus.Read)
         {
            return false;
         }
         if (nextStatus == MessageStatus.Failed)
         {
            return true;
         }
         return Rule[nextStatus] > Rule[currentStatus];
      }
   }
}