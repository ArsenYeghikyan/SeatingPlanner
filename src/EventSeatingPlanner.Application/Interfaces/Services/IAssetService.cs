using EventSeatingPlanner.Application.DTOs.Assets;

namespace EventSeatingPlanner.Application.Interfaces.Services;

public interface IAssetService
{
    Task<AssetDto> CreateAsync(
        Guid ownerUserId,
        Guid? eventId,
        string type,
        string fileName,
        string contentType,
        string storagePath,
        long sizeBytes,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AssetDto>> ListByEventAsync(
        Guid ownerUserId,
        Guid eventId,
        CancellationToken cancellationToken);

    Task<AssetDetailDto?> GetAsync(Guid assetId, CancellationToken cancellationToken);
}
