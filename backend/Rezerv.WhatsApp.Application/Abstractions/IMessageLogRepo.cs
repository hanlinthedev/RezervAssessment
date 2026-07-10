using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Domain.Entities;
using Rezerv.WhatsApp.Domain.Enums;

namespace Rezerv.WhatsApp.Application.Abstractions
{
    public interface IMessageLogRepo
    {
        Task<MessageLog?> GetByMetaMessageIdAsync(string metaMessageId, CancellationToken cancellationToken = default);
        Task<List<MessageLog>> GetByLocationIdAsync(string locationId, MessageStatus? status = null, CancellationToken cancellationToken = default);
        Task CreateAsync(MessageLog messageLog, CancellationToken cancellationToken = default);
        Task UpdateAsync(MessageLog messageLog, CancellationToken cancellationToken = default);
    }
}