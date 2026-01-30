namespace EventSeatingPlanner.Application.DTOs.PrintSettings;

public sealed record UpdatePrintSettingsRequest(
    Guid? BackgroundAssetId,
    string FontKey,
    int TitleFontSize,
    int BodyFontSize,
    string TextColorHex);
