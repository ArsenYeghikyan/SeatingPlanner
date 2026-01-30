using EventSeatingPlanner.Application.Entities;

namespace EventSeatingPlanner.Application.Interfaces.Repositories;

public interface IAssetRepository
{
    Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Asset>> ListByOwnerAsync(Guid ownerUserId, CancellationToken cancellationToken);
    Task AddAsync(Asset asset, CancellationToken cancellationToken);
    Task DeleteAsync(Asset asset, CancellationToken cancellationToken);
}
