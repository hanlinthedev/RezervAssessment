using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Rezerv.WhatsApp.Domain.Entities;

namespace Rezerv.WhatsApp.Infrastructure.Mongo
{
    public class MongoIndexInitializer
    {
        private readonly MongoDBContext _context;

        public MongoIndexInitializer(MongoDBContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await CreateLocationConnectionIndexesAsync(cancellationToken);
            await CreateMessageLogIndexesAsync(cancellationToken);
        }

        private async Task CreateLocationConnectionIndexesAsync(
        CancellationToken cancellationToken)
        {
            var locationIdIndex = new CreateIndexModel<LocationConnection>(
                Builders<LocationConnection>.IndexKeys.Ascending(connection => connection.LocationId),
                new CreateIndexOptions
                {
                    Unique = true,
                    Name = "ux_location_connections_location_id"
                });
            var healthIndex = new CreateIndexModel<LocationConnection>(
                Builders<LocationConnection>.IndexKeys.Ascending(connection => connection.ConnectionState).Ascending(connection => connection.LastActivityAt),
                new CreateIndexOptions { Name = "ix_location_connections_state_last_activity" });

            await _context.LocationConnectionCollection.Indexes.CreateManyAsync(
                new[]
                {
                    locationIdIndex,
                    healthIndex
                },
                cancellationToken: cancellationToken);
        }

        private async Task CreateMessageLogIndexesAsync(
        CancellationToken cancellationToken)
        {
            var metaMessageIdIndex = new CreateIndexModel<MessageLog>(
                Builders<MessageLog>.IndexKeys.Ascending(message => message.MetaMessageId),
                new CreateIndexOptions
                {
                    Name = "ix_message_logs_meta_message_id"
                });

            var locationCreatedAtIndex = new CreateIndexModel<MessageLog>(
                Builders<MessageLog>.IndexKeys
                    .Ascending(message => message.LocationId)
                    .Descending(message => message.CreatedAt),
                new CreateIndexOptions
                {
                    Name = "ix_message_logs_location_id_created_at"
                });

            var locationStatusCreatedAtIndex = new CreateIndexModel<MessageLog>(
                Builders<MessageLog>.IndexKeys
            .Ascending(message => message.LocationId)
            .Ascending(message => message.Status)
            .Descending(message => message.CreatedAt),
            new CreateIndexOptions
            {
                Name = "ix_message_logs_location_id_status_created_at"
            });


            await _context.MessageLogCollection.Indexes.CreateManyAsync(
                new[]
                {
                metaMessageIdIndex,
                locationCreatedAtIndex,
                locationStatusCreatedAtIndex
                },
                cancellationToken);
        }

    }
}