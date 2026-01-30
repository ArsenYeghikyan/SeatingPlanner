namespace EventSeatingPlanner.Application.DTOs;

public sealed record PrintSettingsDto(
    Guid EventId,
    Guid? BackgroundAssetId,
    string FontKey,
    int TitleFontSize,
    int BodyFontSize,
    string TextColorHex);
