using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rezerv.WhatsApp.Application.Abstractions;
using Rezerv.WhatsApp.Infrastructure.BackendJobs;
using Rezerv.WhatsApp.Infrastructure.Meta;
using Rezerv.WhatsApp.Infrastructure.Mongo;
using Rezerv.WhatsApp.Infrastructure.Mongo.Repository;
using Rezerv.WhatsApp.Infrastructure.Seed;
using Rezerv.WhatsApp.Infrastructure.Time;

namespace Rezerv.WhatsApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDBSettings>(configuration.GetSection("MongoDb"));
            services.Configure<MetaWebhookSettings>(configuration.GetSection("MetaWebhook"));

            services.AddSingleton<MongoDBContext>();
            services.AddScoped<ILocationConnectionRepo, LocationConnectionRepo>();
            services.AddScoped<IMessageLogRepo, MessageLogRepo>();
            services.AddSingleton<MongoIndexInitializer>();
            services.AddSingleton<DatabaseSeeder>();

            services.AddSingleton<IClock, SystemClock>();
            services.AddSingleton<IMetaMockWhatsAppClient, MetaMockWhatsAppClient>();
            services.AddSingleton<MetaWebhookSignatureValidator>();

            services.AddHostedService<ConnectionHealthBackgroundService>();

            return services;
        }
    }
}