using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Application.Common;
using Rezerv.WhatsApp.Application.DTO.Webhook;

namespace Rezerv.WhatsApp.Application.Services.MetaWebhook
{
    public interface IMetaWebhookService
    {
        Task<Result> ProcessAsync(MetaWebhookPayload payload, CancellationToken cancellationToken = default);
    }
}