using EventSeatingPlanner.Application.DTOs.Assets;
using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using EventSeatingPlanner.Application.Interfaces.Services;

namespace EventSeatingPlanner.Application.Services;

public sealed class AssetService(IAssetRepository assetRepository) : IAssetService
{
    public async Task<AssetDto> CreateAsync(
        Guid ownerUserId,
        Guid? eventId,
        string type,
        string fileName,
        string contentType,
        string storagePath,
        long sizeBytes,
        CancellationToken cancellationToken)
    {
        var asset = new Asset
        {
            Id = Guid.NewGuid(),
            OwnerUserId = ownerUserId,
            EventId = eventId,
            Type = type,
            FileName = fileName,
            ContentType = contentType,
            StoragePath = storagePath,
            SizeBytes = sizeBytes
        };

        await assetRepository.AddAsync(asset, cancellationToken);

        return new AssetDto(
            asset.Id,
            asset.EventId,
            asset.Type,
            asset.FileName,
            asset.ContentType,
            asset.SizeBytes);
    }

    public async Task<IReadOnlyList<AssetDto>> ListByEventAsync(
        Guid ownerUserId,
        Guid eventId,
        CancellationToken cancellationToken)
    {
        var assets = await assetRepository.ListByOwnerAsync(ownerUserId, cancellationToken);

        return assets
            .Where(asset => asset.EventId == eventId)
            .Select(asset => new AssetDto(
                asset.Id,
                asset.EventId,
                asset.Type,
                asset.FileName,
                asset.ContentType,
                asset.SizeBytes))
            .ToList();
    }

    public async Task<AssetDetailDto?> GetAsync(Guid assetId, CancellationToken cancellationToken)
    {
        var asset = await assetRepository.GetByIdAsync(assetId, cancellationToken);
        if (asset is null)
        {
            return null;
        }

        return new AssetDetailDto(
            asset.Id,
            asset.EventId,
            asset.Type,
            asset.FileName,
            asset.ContentType,
            asset.StoragePath,
            asset.SizeBytes);
    }
}
