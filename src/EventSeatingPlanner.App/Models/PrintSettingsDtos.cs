namespace EventSeatingPlanner.App.Models;

public sealed record PrintSettingsDto(
    Guid EventId,
    Guid? BackgroundAssetId,
    string FontKey,
    int TitleFontSize,
    int BodyFontSize,
    string TextColorHex);

public sealed record UpdatePrintSettingsRequest(
    Guid? BackgroundAssetId,
    string FontKey,
    int TitleFontSize,
    int BodyFontSize,
    string TextColorHex);
