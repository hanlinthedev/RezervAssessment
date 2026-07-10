using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Rezerv.WhatsApp.Application.Abstractions;
using Rezerv.WhatsApp.Domain.Entities;
using Rezerv.WhatsApp.Domain.Enums;

namespace Rezerv.WhatsApp.Infrastructure.Mongo.Repository
{
    public class LocationConnectionRepo : ILocationConnectionRepo
    {
        private readonly IMongoCollection<LocationConnection> _collection;

        public LocationConnectionRepo(MongoDBContext context)
        {
            _collection = context.LocationConnectionCollection;
        }
        public async Task CreateAsync(LocationConnection connection, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(connection, cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyList<LocationConnection>> GetByAllAsync(CancellationToken cancellationToken = default)
        {
            return await _collection.Find(_ => true).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<LocationConnection?> GetByLocationIdAsync(string locationId, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(c => c.LocationId == locationId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(LocationConnection connection, CancellationToken cancellationToken = default)
        {
            var filter = Builders<LocationConnection>.Filter.Eq(c => c.Id, connection.Id);
            await _collection.ReplaceOneAsync(filter, connection, new ReplaceOptions { IsUpsert = true }, cancellationToken: cancellationToken);
        }

        public async Task<int> RefreshConnectionHealthAsync(DateTime staleThreshold, DateTime expiredThreshold, DateTime now, CancellationToken cancellationToken = default)
        {
            var totalModified = 0;

            totalModified += (int)(await MarkExpiredAsync(expiredThreshold, now, cancellationToken)).ModifiedCount;

            totalModified += (int)(await MarkStaleAsync(staleThreshold, expiredThreshold, now, cancellationToken)).ModifiedCount;

            return totalModified;
        }

        private Task<UpdateResult> MarkExpiredAsync(DateTime expiredThreshold, DateTime now, CancellationToken cancellationToken)
        {
            var filter = Builders<LocationConnection>.Filter.And(
                Builders<LocationConnection>.Filter.Ne(connection => connection.ConnectionState, ConnectionState.Disconnected),
                Builders<LocationConnection>.Filter.Ne(connection => connection.ConnectionState, ConnectionState.Expired),
                Builders<LocationConnection>.Filter.Lte(connection => connection.LastActivityAt, expiredThreshold)
            );

            var update = Builders<LocationConnection>.Update
                .Set(connection => connection.ConnectionState, ConnectionState.Expired)
                .Set(connection => connection.UpdatedAt, now);

            return _collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        }

        private Task<UpdateResult> MarkStaleAsync(DateTime staleThreshold, DateTime expiredThreshold, DateTime now, CancellationToken cancellationToken)
        {
            var filter = Builders<LocationConnection>.Filter.And(
                Builders<LocationConnection>.Filter.Ne(connection => connection.ConnectionState, ConnectionState.Disconnected),
                Builders<LocationConnection>.Filter.Ne(connection => connection.ConnectionState, ConnectionState.Stale),
                Builders<LocationConnection>.Filter.Lte(connection => connection.LastActivityAt, staleThreshold),
                Builders<LocationConnection>.Filter.Gt(connection => connection.LastActivityAt, expiredThreshold)
            );

            var update = Builders<LocationConnection>.Update
                .Set(connection => connection.ConnectionState, ConnectionState.Stale)
                .Set(connection => connection.UpdatedAt, now);

            return _collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        }
    }
}