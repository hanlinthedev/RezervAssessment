using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Application.Abstractions;
using Rezerv.WhatsApp.Application.Common;
using Rezerv.WhatsApp.Application.DTO.Connection;
using Rezerv.WhatsApp.Domain.Entities;
using Rezerv.WhatsApp.Domain.Enums;

namespace Rezerv.WhatsApp.Application.Services.Connection
{
    public class LocationConnectionService : ILocationConnectionService
    {
        private readonly ILocationConnectionRepo _locationConnectionRepo;
        private readonly IClock _clock;
        public LocationConnectionService(ILocationConnectionRepo locationConnectionRepo, IClock clock)
        {
            _locationConnectionRepo = locationConnectionRepo;
            _clock = clock;
        }

        public async Task<Result<ConnectionResponse>> CreateAsync(ConnectionRequest request, CancellationToken cancellationToken = default)
        {
            var existingConnection = await _locationConnectionRepo.GetByLocationIdAsync(request.LocationId, cancellationToken);

            if (existingConnection != null)
            {
                return Result<ConnectionResponse>.Failure("A connection for this location already exists.", ErrorType.Conflict);
            }

            var now = _clock.UtcNow;
            var newConnection = new LocationConnection
            {
                Id = Guid.NewGuid().ToString(),
                LocationId = request.LocationId,
                PhoneNumber = request.PhoneNumber,
                DisplayName = request.DisplayName,
                ConnectionState = ConnectionState.Active,
                LastActivityAt = request.LastActivityAt ?? now,
                ConnectedAt = now,
                UpdatedAt = now
            };

            await _locationConnectionRepo.CreateAsync(newConnection, cancellationToken);

            var response = ToResponse(newConnection);

            return Result<ConnectionResponse>.Success(response);
        }

        public async Task<Result<ConnectionResponse>> GetByLocationIdAsync(string locationId, CancellationToken cancellationToken = default)
        {
            var connection = await _locationConnectionRepo.GetByLocationIdAsync(locationId, cancellationToken);

            if (connection == null)
            {
                return Result<ConnectionResponse>.Failure("Connection not found.", ErrorType.NotFound);
            }

            var response = ToResponse(connection);
            return Result<ConnectionResponse>.Success(response);
        }

        public async Task<Result<IReadOnlyList<ConnectionResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var connections = await _locationConnectionRepo.GetByAllAsync(cancellationToken);
            var responseList = connections.Select(ToResponse).ToList();
            return Result<IReadOnlyList<ConnectionResponse>>.Success(responseList);
        }

        public async Task<Result> DisconnectAsync(string locationId, CancellationToken cancellationToken = default)
        {
            var connection = await _locationConnectionRepo.GetByLocationIdAsync(locationId, cancellationToken);

            if (connection == null)
            {
                return Result.Failure("Connection not found.", ErrorType.NotFound);
            }

            connection.ConnectionState = ConnectionState.Disconnected;
            connection.UpdatedAt = _clock.UtcNow;

            await _locationConnectionRepo.UpdateAsync(connection, cancellationToken);

            return Result.Success();
        }

        public async Task<Result<ConnectionResponse>> ReconnectAsync(string locationId, CancellationToken cancellationToken = default)
        {
            var connection = await _locationConnectionRepo.GetByLocationIdAsync(locationId, cancellationToken);

            if (connection == null)
            {
                return Result<ConnectionResponse>.Failure("Connection not found.", ErrorType.NotFound);
            }
            if (connection.ConnectionState is ConnectionState.Active or ConnectionState.Stale)
            {
                return Result<ConnectionResponse>.Failure($"Can't reconnect because connection state is already {connection.ConnectionState}", ErrorType.Validation);
            }
            var now = _clock.UtcNow;
            connection.ConnectionState = ConnectionState.Active;
            connection.ConnectedAt = now;
            connection.LastActivityAt = now;
            connection.UpdatedAt = now;

            await _locationConnectionRepo.UpdateAsync(connection, cancellationToken);

            var response = ToResponse(connection);
            return Result<ConnectionResponse>.Success(response);
        }

        public async Task<int> RefreshConnectionHealthAsync(CancellationToken cancellationToken = default)
        {
            var now = _clock.UtcNow;

            var staleThreshold = now.AddDays(-8);
            var expiredThreshold = now.AddDays(-12);

            return await _locationConnectionRepo.RefreshConnectionHealthAsync(staleThreshold, expiredThreshold, now, cancellationToken);
        }

        private ConnectionResponse ToResponse(LocationConnection connection)
        {
            return new ConnectionResponse
            {
                Id = connection.Id,
                LocationId = connection.LocationId,
                PhoneNumber = connection.PhoneNumber,
                DisplayName = connection.DisplayName,
                ConnectionState = connection.ConnectionState,
                LastActivityAt = connection.LastActivityAt,
                ConnectedAt = connection.ConnectedAt,
                UpdatedAt = connection.UpdatedAt
            };
        }
    }
}