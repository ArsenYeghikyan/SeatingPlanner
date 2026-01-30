using EventSeatingPlanner.Application.Interfaces.Services;

namespace EventSeatingPlanner.Infrastructure.Storage;

public sealed class InMemoryAssetStorage : IAssetStorage
{
    private readonly Dictionary<string, AssetContent> _storage = new();

    public Task<AssetStorageResult> SaveAsync(
        string fileName,
        string contentType,
        Stream content,
        CancellationToken cancellationToken)
    {
        var storagePath = $"asset-{Guid.NewGuid():N}";
        using var memoryStream = new MemoryStream();
        content.CopyTo(memoryStream);

        var assetContent = new AssetContent(contentType, memoryStream.ToArray());
        _storage[storagePath] = assetContent;

        return Task.FromResult(new AssetStorageResult(storagePath, assetContent.Content.Length));
    }

    public Task<AssetContent?> GetAsync(string storagePath, CancellationToken cancellationToken)
    {
        _storage.TryGetValue(storagePath, out var content);
        return Task.FromResult(content);
    }

    public Task DeleteAsync(string storagePath, CancellationToken cancellationToken)
    {
        _storage.Remove(storagePath);
        return Task.CompletedTask;
    }
}
