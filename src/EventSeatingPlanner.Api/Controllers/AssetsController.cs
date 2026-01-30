using EventSeatingPlanner.Application.DTOs.Assets;
using EventSeatingPlanner.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventSeatingPlanner.Api.Controllers;

[ApiController]
public sealed class AssetsController : ControllerBase
{
    private static readonly Guid DemoOwnerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private readonly IAssetService _assetService;
    private readonly IAssetStorage _assetStorage;

    public AssetsController(IAssetService assetService, IAssetStorage assetStorage)
    {
        _assetService = assetService;
        _assetStorage = assetStorage;
    }

    [HttpGet("api/events/{eventId:guid}/assets")]
    public async Task<ActionResult<IReadOnlyList<AssetDto>>> List(Guid eventId, CancellationToken cancellationToken)
    {
        var assets = await _assetService.ListByEventAsync(DemoOwnerId, eventId, cancellationToken);
        return Ok(assets);
    }

    [HttpPost("api/events/{eventId:guid}/assets/{assetType}")]
    public async Task<ActionResult<AssetDto>> Upload(
        Guid eventId,
        string assetType,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file.Length == 0)
        {
            return BadRequest("Файл пуст.");
        }

        await using var stream = file.OpenReadStream();
        var storageResult = await _assetStorage.SaveAsync(file.FileName, file.ContentType, stream, cancellationToken);
        var asset = await _assetService.CreateAsync(
            DemoOwnerId,
            eventId,
            assetType,
            file.FileName,
            file.ContentType,
            storageResult.StoragePath,
            storageResult.SizeBytes,
            cancellationToken);

        return CreatedAtAction(nameof(Download), new { assetId = asset.Id }, asset);
    }

    [HttpGet("api/assets/{assetId:guid}")]
    public async Task<IActionResult> Download(Guid assetId, CancellationToken cancellationToken)
    {
        var asset = await _assetService.GetAsync(assetId, cancellationToken);
        if (asset is null)
        {
            return NotFound();
        }

        var content = await _assetStorage.GetAsync(asset.StoragePath, cancellationToken);
        if (content is null)
        {
            return NotFound();
        }

        return File(content.Content, content.ContentType, asset.FileName);
    }
}
