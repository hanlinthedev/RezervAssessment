using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Application.DTO.Meta;

namespace Rezerv.WhatsApp.Application.Abstractions
{
    public interface IMetaMockWhatsAppClient
    {
        Task<MetaSendMessageResult> SendMessageAsync(string senderPhoneNumber, string recipientPhone, string messageContent, CancellationToken cancellationToken = default);
    }
}