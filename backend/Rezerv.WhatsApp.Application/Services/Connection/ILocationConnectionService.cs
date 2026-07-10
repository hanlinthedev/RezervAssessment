using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Application.Common;
using Rezerv.WhatsApp.Application.DTO.Connection;

namespace Rezerv.WhatsApp.Application.Services.Connection
{
    public interface ILocationConnectionService
    {
        Task<Result<ConnectionResponse>> CreateAsync(ConnectionRequest request, CancellationToken cancellationToken = default);
        Task<Result<ConnectionResponse>> GetByLocationIdAsync(string locationId, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<ConnectionResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result> DisconnectAsync(string locationId, CancellationToken cancellationToken = default);
        Task<Result<ConnectionResponse>> ReconnectAsync(string locationId, CancellationToken cancellationToken = default);
        Task<int> RefreshConnectionHealthAsync(CancellationToken cancellationToken = default);
    }
}