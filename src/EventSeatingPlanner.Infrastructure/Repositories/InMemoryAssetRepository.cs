using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class InMemoryAssetRepository : IAssetRepository
{
    private readonly List<Asset> _assets = new();

    public Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_assets.FirstOrDefault(asset => asset.Id == id));
    }

    public Task<IReadOnlyList<Asset>> ListByOwnerAsync(Guid ownerUserId, CancellationToken cancellationToken)
    {
        IReadOnlyList<Asset> assets = _assets
            .Where(asset => asset.OwnerUserId == ownerUserId)
            .ToList();
        return Task.FromResult(assets);
    }

    public Task AddAsync(Asset asset, CancellationToken cancellationToken)
    {
        _assets.Add(asset);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Asset asset, CancellationToken cancellationToken)
    {
        _assets.RemoveAll(existing => existing.Id == asset.Id);
        return Task.CompletedTask;
    }
}
