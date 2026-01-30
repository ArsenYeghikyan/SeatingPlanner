using EventSeatingPlanner.Application.DTOs;
using EventSeatingPlanner.Application.DTOs.PrintSettings;
using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using EventSeatingPlanner.Application.Interfaces.Services;

namespace EventSeatingPlanner.Application.Services;

public sealed class PrintSettingsService(IPrintSettingsRepository printSettingsRepository) : IPrintSettingsService
{
    public async Task<PrintSettingsDto> GetAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var settings = await printSettingsRepository.GetByEventIdAsync(eventId, cancellationToken)
                       ?? new EventPrintSettings
                       {
                           EventId = eventId,
                           FontKey = "Default",
                           TitleFontSize = 24,
                           BodyFontSize = 12,
                           TextColorHex = "#000000",
                           UpdatedAt = DateTimeOffset.UtcNow
                       };

        return new PrintSettingsDto(
            settings.EventId,
            settings.BackgroundAssetId,
            settings.FontKey,
            settings.TitleFontSize,
            settings.BodyFontSize,
            settings.TextColorHex);
    }

    public async Task<PrintSettingsDto> UpdateAsync(
        Guid eventId,
        UpdatePrintSettingsRequest request,
        CancellationToken cancellationToken)
    {
        var settings = new EventPrintSettings
        {
            EventId = eventId,
            BackgroundAssetId = request.BackgroundAssetId,
            FontKey = request.FontKey,
            TitleFontSize = request.TitleFontSize,
            BodyFontSize = request.BodyFontSize,
            TextColorHex = request.TextColorHex,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await printSettingsRepository.UpsertAsync(settings, cancellationToken);

        return new PrintSettingsDto(
            settings.EventId,
            settings.BackgroundAssetId,
            settings.FontKey,
            settings.TitleFontSize,
            settings.BodyFontSize,
            settings.TextColorHex);
    }
}
