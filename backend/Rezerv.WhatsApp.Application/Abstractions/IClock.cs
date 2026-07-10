using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezerv.WhatsApp.Application.Abstractions
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}