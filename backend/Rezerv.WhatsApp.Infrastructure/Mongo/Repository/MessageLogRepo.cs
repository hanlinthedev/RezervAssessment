using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Application.Abstractions;
using MongoDB.Driver;
using Rezerv.WhatsApp.Domain.Entities;
using Rezerv.WhatsApp.Domain.Enums;

namespace Rezerv.WhatsApp.Infrastructure.Mongo.Repository
{
    public class MessageLogRepo : IMessageLogRepo
    {
        private readonly IMongoCollection<MessageLog> _collection;
        public MessageLogRepo(MongoDBContext context)
        {
            _collection = context.MessageLogCollection;
        }

        public async Task CreateAsync(MessageLog messageLog, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(messageLog, cancellationToken: cancellationToken);
        }

        public async Task<List<MessageLog>> GetByLocationIdAsync(string locationId, MessageStatus? status = null, CancellationToken cancellationToken = default)
        {
            var filter = Builders<MessageLog>.Filter.Eq(m => m.LocationId, locationId);
            if (status != null)
            {
                filter = Builders<MessageLog>.Filter.And(filter, Builders<MessageLog>.Filter.Eq(m => m.Status, status));
            }
            return await _collection.Find(filter).SortByDescending(message => message.CreatedAt).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<MessageLog?> GetByMetaMessageIdAsync(string metaMessageId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<MessageLog>.Filter.Eq(m => m.MetaMessageId, metaMessageId);
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(MessageLog messageLog, CancellationToken cancellationToken = default)
        {
            var filter = Builders<MessageLog>.Filter.Eq(m => m.Id, messageLog.Id);
            await _collection.ReplaceOneAsync(filter, messageLog, new ReplaceOptions { IsUpsert = true }, cancellationToken: cancellationToken);
        }
    }
}