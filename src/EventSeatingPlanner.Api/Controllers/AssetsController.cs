using EventSeatingPlanner.Application.DTOs.Assets;
using EventSeatingPlanner.Application.Interfaces.Services;
using EventSeatingPlanner.Api.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventSeatingPlanner.Api.Controllers;

[ApiController]
[Authorize]
public sealed class AssetsController : ControllerBase
{
    private readonly IAssetService _assetService;
    private readonly IAssetStorage _assetStorage;
    private readonly IPrintSettingsService _printSettingsService;

    public AssetsController(
        IAssetService assetService,
        IAssetStorage assetStorage,
        IPrintSettingsService printSettingsService)
    {
        _assetService = assetService;
        _assetStorage = assetStorage;
        _printSettingsService = printSettingsService;
    }

    [HttpGet("api/events/{eventId:guid}/assets")]
    public async Task<ActionResult<IReadOnlyList<AssetDto>>> List(Guid eventId, CancellationToken cancellationToken)
    {
        var assets = await _assetService.ListByEventAsync(User.GetUserId(), eventId, cancellationToken);
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
            User.GetUserId(),
            eventId,
            assetType,
            file.FileName,
            file.ContentType,
            storageResult.StoragePath,
            storageResult.SizeBytes,
            cancellationToken);

        if (assetType.Equals("background", StringComparison.OrdinalIgnoreCase))
        {
            await _printSettingsService.UpdateBackgroundAsync(eventId, asset.Id, cancellationToken);
        }

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

        return File(content.Content, asset.ContentType, asset.FileName);
    }
}
