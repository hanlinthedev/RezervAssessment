using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Application.Abstractions;
using Rezerv.WhatsApp.Application.Common;
using Rezerv.WhatsApp.Application.DTO.Messages;
using Rezerv.WhatsApp.Domain.Entities;
using Rezerv.WhatsApp.Domain.Enums;

namespace Rezerv.WhatsApp.Application.Services.Message
{
    public class MessageLogService : IMessageLogService
    {
        private readonly IMessageLogRepo _messageLogRepo;
        private readonly ILocationConnectionRepo _locationConnectionRepo;
        private readonly IMetaMockWhatsAppClient _metaWhatsAppClient;
        private readonly IClock _clock;
        public MessageLogService(IMessageLogRepo messageLogRepo, ILocationConnectionRepo locationConnectionRepo, IMetaMockWhatsAppClient metaWhatsAppClient, IClock clock)
        {
            _messageLogRepo = messageLogRepo;
            _locationConnectionRepo = locationConnectionRepo;
            _metaWhatsAppClient = metaWhatsAppClient;
            _clock = clock;
        }
        public async Task<Result<MessageLogResponse>> SendAsync(SendMessageRequest request, CancellationToken cancellationToken = default)
        {
            var connection = await _locationConnectionRepo.GetByLocationIdAsync(request.LocationId, cancellationToken);
            if (connection is null)
            {
                return Result<MessageLogResponse>.Failure("WhatsApp connection not found for this location.", ErrorType.NotFound);
            }
            if (connection.ConnectionState is ConnectionState.Expired or ConnectionState.Disconnected)
            {
                return Result<MessageLogResponse>.Failure($"Message cannot be sent because the WhatsApp connection is {connection.ConnectionState}.", ErrorType.BusinessRule);
            }

            var now = _clock.UtcNow;
            var messageLog = new MessageLog
            {
                Id = Guid.NewGuid().ToString(),
                LocationId = request.LocationId,
                RecipientPhone = request.RecipientPhone,
                MessageContent = request.MessageContent,
                Status = MessageStatus.Queued,
                CreatedAt = now
            };

            await _messageLogRepo.CreateAsync(messageLog, cancellationToken);

            var metaResult = await _metaWhatsAppClient.SendMessageAsync(senderPhoneNumber: connection.PhoneNumber, recipientPhone: request.RecipientPhone, messageContent: request.MessageContent, cancellationToken: cancellationToken);

            if (metaResult.IsSuccess)
            {
                messageLog.Status = MessageStatus.Sent;
                messageLog.MetaMessageId = metaResult.MetaMessageId;
                messageLog.SentAt = now;
            }
            else
            {
                messageLog.Status = MessageStatus.Failed;
                messageLog.FailedAt = now;
                messageLog.FailureReason = metaResult.ErrorMessage ?? "Meta API send failed.";
            }

            await _messageLogRepo.UpdateAsync(messageLog, cancellationToken);

            return Result<MessageLogResponse>.Success(ToResponse(messageLog));
        }

        public async Task<Result<IReadOnlyList<MessageLogResponse>>> GetByLocationIdAsync(string locationId, MessageStatus? status = null, CancellationToken cancellationToken = default)
        {
            var messageLogs = await _messageLogRepo.GetByLocationIdAsync(locationId, status, cancellationToken);
            var responseList = messageLogs.Select(ToResponse).ToList();
            return Result<IReadOnlyList<MessageLogResponse>>.Success(responseList);
        }

        private static MessageLogResponse ToResponse(MessageLog messageLog)
        {
            return new MessageLogResponse
            {
                Id = messageLog.Id,
                LocationId = messageLog.LocationId,
                RecipientPhone = messageLog.RecipientPhone,
                MessageContent = messageLog.MessageContent,
                MetaMessageId = messageLog.MetaMessageId,
                Status = messageLog.Status,
                CreatedAt = messageLog.CreatedAt,
                SentAt = messageLog.SentAt,
                DeliveredAt = messageLog.DeliveredAt,
                ReadAt = messageLog.ReadAt,
                FailedAt = messageLog.FailedAt,
                FailureReason = messageLog.FailureReason
            };
        }
    }
}