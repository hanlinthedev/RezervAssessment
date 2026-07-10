using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Application.Abstractions;

namespace Rezerv.WhatsApp.Infrastructure.Time
{
    public class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}