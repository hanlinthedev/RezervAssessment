using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Rezerv.WhatsApp.Domain.Entities;
using Rezerv.WhatsApp.Domain.Enums;
using Rezerv.WhatsApp.Infrastructure.Mongo;

namespace Rezerv.WhatsApp.Infrastructure.Seed
{
    public class DatabaseSeeder
    {
        private readonly MongoDBContext _context;

        public DatabaseSeeder(MongoDBContext context)
        {
            _context = context;
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            var existingConnectionsCount = await _context.LocationConnectionCollection
                .CountDocumentsAsync(Builders<LocationConnection>.Filter.Empty, cancellationToken: cancellationToken);

            var existingMessagesCount = await _context.MessageLogCollection
                .CountDocumentsAsync(Builders<MessageLog>.Filter.Empty, cancellationToken: cancellationToken);

            if (existingConnectionsCount > 0 || existingMessagesCount > 0)
            {
                return;
            }

            var now = DateTime.UtcNow;

            var connections = new List<LocationConnection>
            {
                CreateConnection(
                    "loc-active-001",
                    "+959111111111",
                    "Yangon Active Studio",
                    ConnectionState.Active,
                    now.AddDays(-1),
                    now.AddDays(-20),
                    now),

                CreateConnection(
                    "loc-stale-001",
                    "+959222222222",
                    "Mandalay Stale Studio",
                    ConnectionState.Stale,
                    now.AddDays(-9),
                    now.AddDays(-30),
                    now),

                CreateConnection(
                    "loc-expired-001",
                    "+959333333333",
                    "Bago Expired Studio",
                    ConnectionState.Expired,
                    now.AddDays(-14),
                    now.AddDays(-40),
                    now),

                CreateConnection(
                    "loc-disconnected-001",
                    "+959444444444",
                    "Taunggyi Disconnected Studio",
                    ConnectionState.Disconnected,
                    now.AddDays(-3),
                    now.AddDays(-25),
                    now),
                CreateConnection(
                    "loc-stale-bg-test-001",
                    "+959555555551",
                    "Naypyidaw Stale Background Test Studio",
                    ConnectionState.Active,
                    now.AddDays(-9),
                    now.AddDays(-20),
                    now),
                CreateConnection(
                    "loc-expired-bg-test-001",
                    "+959555555552",
                    "Mawlamyine Expired Background Test Studio",
                    ConnectionState.Active,
                    now.AddDays(-14),
                    now.AddDays(-25),
                    now),
            };

            await _context.LocationConnectionCollection.InsertManyAsync(connections, cancellationToken: cancellationToken);

            var messageLogs = new List<MessageLog>
            {
                CreateMessage(
                    "loc-active-001",
                    "+959900000001",
                    "Your booking is queued.",
                    MessageStatus.Queued,
                    now.AddMinutes(-50)),

                CreateMessage(
                    "loc-active-001",
                    "+959900000002",
                    "Your booking is confirmed.",
                    MessageStatus.Sent,
                    now.AddMinutes(-45),
                    metaMessageId: "wamid.seed.sent.001",
                    sentAt: now.AddMinutes(-44)),

                CreateMessage(
                    "loc-active-001",
                    "+959900000003",
                    "Your class starts soon.",
                    MessageStatus.Delivered,
                    now.AddMinutes(-40),
                    metaMessageId: "wamid.seed.delivered.001",
                    sentAt: now.AddMinutes(-39),
                    deliveredAt: now.AddMinutes(-38)),

                CreateMessage(
                    "loc-active-001",
                    "+959900000004",
                    "Thanks for visiting.",
                    MessageStatus.Read,
                    now.AddMinutes(-35),
                    metaMessageId: "wamid.seed.read.001",
                    sentAt: now.AddMinutes(-34),
                    deliveredAt: now.AddMinutes(-33),
                    readAt: now.AddMinutes(-32)),

                CreateMessage(
                    "loc-active-001",
                    "+959900000005",
                    "This message failed.",
                    MessageStatus.Failed,
                    now.AddMinutes(-30),
                    metaMessageId: "wamid.seed.failed.001",
                    failedAt: now.AddMinutes(-29),
                    failureReason: "Seeded failed message."),

                CreateMessage(
                    "loc-stale-001",
                    "+959900000006",
                    "Stale location sent message.",
                    MessageStatus.Sent,
                    now.AddMinutes(-25),
                    metaMessageId: "wamid.seed.sent.002",
                    sentAt: now.AddMinutes(-24)),

                CreateMessage(
                    "loc-stale-001",
                    "+959900000007",
                    "Stale location delivered message.",
                    MessageStatus.Delivered,
                    now.AddMinutes(-20),
                    metaMessageId: "wamid.seed.delivered.002",
                    sentAt: now.AddMinutes(-19),
                    deliveredAt: now.AddMinutes(-18)),

                CreateMessage(
                    "loc-expired-001",
                    "+959900000008",
                    "Old expired location message.",
                    MessageStatus.Failed,
                    now.AddMinutes(-15),
                    metaMessageId: "wamid.seed.expired.failed.001",
                    failedAt: now.AddMinutes(-14),
                    failureReason: "Connection expired."),

                CreateMessage(
                    "loc-disconnected-001",
                    "+959900000009",
                    "Old disconnected location message.",
                    MessageStatus.Failed,
                    now.AddMinutes(-10),
                    metaMessageId: "wamid.seed.disconnected.failed.001",
                    failedAt: now.AddMinutes(-9),
                    failureReason: "Connection disconnected."),

                CreateMessage(
                    "loc-active-001",
                    "+959900000010",
                    "Another read message.",
                    MessageStatus.Read,
                    now.AddMinutes(-5),
                    metaMessageId: "wamid.seed.read.002",
                    sentAt: now.AddMinutes(-4),
                    deliveredAt: now.AddMinutes(-3),
                    readAt: now.AddMinutes(-2))
            };

            await _context.MessageLogCollection.InsertManyAsync(messageLogs, cancellationToken: cancellationToken);
        }

        private static LocationConnection CreateConnection(
            string locationId,
            string phoneNumber,
            string displayName,
            ConnectionState connectionState,
            DateTime lastActivityAt,
            DateTime connectedAt,
            DateTime updatedAt)
        {
            return new LocationConnection
            {
                Id = Guid.NewGuid().ToString(),
                LocationId = locationId,
                PhoneNumber = phoneNumber,
                DisplayName = displayName,
                ConnectionState = connectionState,
                LastActivityAt = lastActivityAt,
                ConnectedAt = connectedAt,
                UpdatedAt = updatedAt
            };
        }

        private static MessageLog CreateMessage(
            string locationId,
            string recipientPhone,
            string content,
            MessageStatus status,
            DateTime createdAt,
            string? metaMessageId = null,
            DateTime? sentAt = null,
            DateTime? deliveredAt = null,
            DateTime? readAt = null,
            DateTime? failedAt = null,
            string? failureReason = null)
        {
            return new MessageLog
            {
                Id = Guid.NewGuid().ToString(),
                LocationId = locationId,
                RecipientPhone = recipientPhone,
                MessageContent = content,
                MetaMessageId = metaMessageId,
                Status = status,
                CreatedAt = createdAt,
                SentAt = sentAt,
                DeliveredAt = deliveredAt,
                ReadAt = readAt,
                FailedAt = failedAt,
                FailureReason = failureReason
            };
        }
    }

}