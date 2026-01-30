using EventSeatingPlanner.Application.Interfaces.Services;

namespace EventSeatingPlanner.Infrastructure.Storage;

public sealed class FileSystemAssetStorage : IAssetStorage
{
    private readonly string _rootPath;

    public FileSystemAssetStorage(string rootPath)
    {
        _rootPath = rootPath ?? throw new ArgumentNullException(nameof(rootPath));
        Directory.CreateDirectory(_rootPath);
    }

    public async Task<AssetStorageResult> SaveAsync(
        string fileName,
        string contentType,
        Stream content,
        CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(fileName);
        var storagePath = $"{Guid.NewGuid():N}{extension}";
        var fullPath = ResolvePath(storagePath);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? _rootPath);

        await using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await content.CopyToAsync(fileStream, cancellationToken);

        var fileInfo = new FileInfo(fullPath);
        return new AssetStorageResult(storagePath, fileInfo.Length);
    }

    public async Task<AssetContent?> GetAsync(string storagePath, CancellationToken cancellationToken)
    {
        var fullPath = ResolvePath(storagePath);
        if (!File.Exists(fullPath))
        {
            return null;
        }

        var bytes = await File.ReadAllBytesAsync(fullPath, cancellationToken);
        return new AssetContent(ResolveContentType(fullPath), bytes);
    }

    public Task DeleteAsync(string storagePath, CancellationToken cancellationToken)
    {
        var fullPath = ResolvePath(storagePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    private string ResolvePath(string storagePath)
    {
        if (Path.IsPathRooted(storagePath))
        {
            return storagePath;
        }

        return Path.Combine(_rootPath, storagePath);
    }

    private static string ResolveContentType(string path)
    {
        return Path.GetExtension(path).ToLowerInvariant() switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            _ => "application/octet-stream"
        };
    }
}
