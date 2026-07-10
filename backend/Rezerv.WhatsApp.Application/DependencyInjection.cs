using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rezerv.WhatsApp.Application.Services.Connection;
using Rezerv.WhatsApp.Application.Services.Message;
using Rezerv.WhatsApp.Application.Services.MetaWebhook;

namespace Rezerv.WhatsApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ILocationConnectionService, LocationConnectionService>();
            services.AddScoped<IMessageLogService, MessageLogService>();
            services.AddScoped<IMetaWebhookService, MetaWebhookService>();

            return services;
        }
    }
}