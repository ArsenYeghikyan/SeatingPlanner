namespace EventSeatingPlanner.Application.Interfaces.Services;

public interface IAssetStorage
{
    Task<AssetStorageResult> SaveAsync(
        string fileName,
        string contentType,
        Stream content,
        CancellationToken cancellationToken);

    Task<AssetContent?> GetAsync(string storagePath, CancellationToken cancellationToken);
    Task DeleteAsync(string storagePath, CancellationToken cancellationToken);
}

public sealed record AssetStorageResult(string StoragePath, long SizeBytes);

public sealed record AssetContent(string ContentType, byte[] Content);
