using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rezerv.WhatsApp.Application.Services.Connection;

namespace Rezerv.WhatsApp.Infrastructure.BackendJobs
{
    public class ConnectionHealthBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ConnectionHealthBackgroundService> _logger;

        public ConnectionHealthBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<ConnectionHealthBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await RefreshConnectionHealthAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to refresh WhatsApp connection health.");
                }

                await timer.WaitForNextTickAsync(stoppingToken);
            }
        }

        private async Task RefreshConnectionHealthAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var connectionService = scope.ServiceProvider
                .GetRequiredService<ILocationConnectionService>();

            var updatedCount = await connectionService.RefreshConnectionHealthAsync(
                cancellationToken);

            _logger.LogInformation(
                "WhatsApp connection health refresh completed. Updated {UpdatedCount} connections.",
                updatedCount);
        }
    }
}