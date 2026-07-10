using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Application.Abstractions;
using Rezerv.WhatsApp.Application.Common;
using Rezerv.WhatsApp.Application.DTO.Webhook;
using Rezerv.WhatsApp.Domain.Enums;
using Rezerv.WhatsApp.Domain.Policies;
using Microsoft.Extensions.Logging;

namespace Rezerv.WhatsApp.Application.Services.MetaWebhook
{
    public class MetaWebhookService : IMetaWebhookService
    {
        private readonly IMessageLogRepo _messageLogRepo;
        private readonly ILogger<MetaWebhookService> _logger;
        public MetaWebhookService(IMessageLogRepo messageLogRepo, ILogger<MetaWebhookService> logger)
        {
            _messageLogRepo = messageLogRepo;
            _logger = logger;
        }
        public async Task<Result> ProcessAsync(MetaWebhookPayload payload, CancellationToken cancellationToken = default)
        {
            try
            {
                var statuses = ExtractMessageStatuses(payload);

                foreach (var statusUpdate in statuses)
                {
                    await ProcessTaskAsync(statusUpdate, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Web Hook process failed but swallowed errors here");
            }

            return Result.Success();
        }

        private async Task ProcessTaskAsync(MetaWebhookStatus statusUpdate, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(statusUpdate.Id))
                return;
            if (!TryParseStatus(statusUpdate.Status, out var nextStatus))
                return;

            var messageLog = await _messageLogRepo.GetByMetaMessageIdAsync(statusUpdate.Id, cancellationToken);
            if (messageLog is null)
                return;

            if (!MessageStatusLifeCyclePolicy.ShouldApplyTransition(messageLog.Status, nextStatus))
                return;

            messageLog.Status = nextStatus;

            var eventTime = ParseWebhookTimestamp(statusUpdate.Timestamp);

            switch (nextStatus)
            {
                case MessageStatus.Sent:
                    messageLog.SentAt = eventTime;
                    break;

                case MessageStatus.Delivered:
                    messageLog.DeliveredAt = eventTime;
                    break;

                case MessageStatus.Read:
                    messageLog.ReadAt = eventTime;
                    break;

                case MessageStatus.Failed:
                    messageLog.FailedAt = eventTime;
                    messageLog.FailureReason = "Failed status received from Meta webhook.";
                    break;
            }

            await _messageLogRepo.UpdateAsync(messageLog, cancellationToken);
        }

        private static IReadOnlyList<MetaWebhookStatus> ExtractMessageStatuses(MetaWebhookPayload payload)
        {
            var statuses = new List<MetaWebhookStatus>();

            statuses = payload.Entry
                .SelectMany(entry => entry.Changes)
                .Select(change => change.Value)
                .Where(value => value is not null)
                .SelectMany(value => value!.Statuses)
                .ToList();

            return statuses;
        }

        private static bool TryParseStatus(string status, out MessageStatus messageStatus)
        {
            messageStatus = default;

            if (string.IsNullOrWhiteSpace(status))
                return false;

            switch (status.ToLowerInvariant())
            {
                case "sent":
                    messageStatus = MessageStatus.Sent;
                    return true;
                case "delivered":
                    messageStatus = MessageStatus.Delivered;
                    return true;
                case "read":
                    messageStatus = MessageStatus.Read;
                    return true;
                case "failed":
                    messageStatus = MessageStatus.Failed;
                    return true;
                default:
                    return false;
            }
        }

        private static DateTime ParseWebhookTimestamp(string timestamp)
        {
            if (!long.TryParse(timestamp, out var seconds))
                return DateTime.UtcNow;

            return DateTimeOffset
                .FromUnixTimeSeconds(seconds)
                .UtcDateTime;
        }
    }
}