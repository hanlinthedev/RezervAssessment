using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Domain.Enums;

namespace Rezerv.WhatsApp.Domain.Policies
{
    public class ConnectionHealthPolicy
    {
        public static ConnectionState CalculateState(
        ConnectionState currentState,
        DateTime lastActivityAt,
        DateTime now)
        {
            if (currentState == ConnectionState.Disconnected)
                return ConnectionState.Disconnected;

            var inactiveDays = (now - lastActivityAt).TotalDays;

            if (inactiveDays > 12)
                return ConnectionState.Expired;

            if (inactiveDays > 8)
                return ConnectionState.Stale;

            return ConnectionState.Active;
        }
    }
}