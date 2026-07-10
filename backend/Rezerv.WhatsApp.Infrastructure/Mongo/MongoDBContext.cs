using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Rezerv.WhatsApp.Domain.Entities;

namespace Rezerv.WhatsApp.Infrastructure.Mongo
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<LocationConnection> LocationConnectionCollection => _database.GetCollection<LocationConnection>("location_connections");

        public IMongoCollection<MessageLog> MessageLogCollection => _database.GetCollection<MessageLog>("message_logs");
    }
}