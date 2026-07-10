using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Application.Abstractions;
using Rezerv.WhatsApp.Application.Common;
using Rezerv.WhatsApp.Application.DTO.Meta;

namespace Rezerv.WhatsApp.Infrastructure.Meta
{
    public class MetaMockWhatsAppClient : IMetaMockWhatsAppClient
    {
        private int _callCount;

        public Task<MetaSendMessageResult> SendMessageAsync(
            string senderPhoneNumber,
            string recipientPhone,
            string messageContent,
            CancellationToken cancellationToken = default)
        {
            var currentCall = Interlocked.Increment(ref _callCount);

            if (currentCall % 5 == 0)
            {
                return Task.FromResult(
                    MetaSendMessageResult.Failure("Simulated Meta API failure."));
            }

            var metaMessageId = $"wamid.mock.{Guid.NewGuid():N}";

            return Task.FromResult(
                MetaSendMessageResult.Success(metaMessageId));
        }
    }
}