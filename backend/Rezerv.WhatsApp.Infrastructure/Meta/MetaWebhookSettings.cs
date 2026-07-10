using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezerv.WhatsApp.Infrastructure.Meta
{
    public class MetaWebhookSettings
    {
        public string VerifyToken { get; init; } = default!;
        public string AppSecret { get; init; } = default!;
    }
}