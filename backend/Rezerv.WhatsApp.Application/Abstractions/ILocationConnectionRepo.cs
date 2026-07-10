using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezerv.WhatsApp.Domain.Entities;

namespace Rezerv.WhatsApp.Application.Abstractions
{
    public interface ILocationConnectionRepo
    {
        Task<LocationConnection?> GetByLocationIdAsync(string locationId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<LocationConnection>> GetByAllAsync(CancellationToken cancellationToken = default);
        Task CreateAsync(LocationConnection connection, CancellationToken cancellationToken = default);
        Task UpdateAsync(LocationConnection connection, CancellationToken cancellationToken = default);
        Task<int> RefreshConnectionHealthAsync(DateTime staleThreshold, DateTime expiredThreshold, DateTime now, CancellationToken cancellationToken = default);
    }
}