using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Application.Common;
using Rezerv.WhatsApp.Application.DTO.Messages;
using Rezerv.WhatsApp.Domain.Enums;

namespace Rezerv.WhatsApp.Application.Services.Message
{
    public interface IMessageLogService
    {
        Task<Result<MessageLogResponse>> SendAsync(SendMessageRequest request, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<MessageLogResponse>>> GetByLocationIdAsync(string locationId, MessageStatus? status = null, CancellationToken cancellationToken = default);
    }
}