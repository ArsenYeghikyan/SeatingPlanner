namespace EventSeatingPlanner.App.Models;

public sealed record PrintSettingsDto(
    Guid EventId,
    Guid? BackgroundAssetId,
    string FontKey,
    int TitleFontSize,
    int BodyFontSize,
    string TextColorHex);

public sealed class UpdatePrintSettingsRequest
{
    public UpdatePrintSettingsRequest(
        Guid? backgroundAssetId,
        string fontKey,
        int titleFontSize,
        int bodyFontSize,
        string textColorHex)
    {
        BackgroundAssetId = backgroundAssetId;
        FontKey = fontKey;
        TitleFontSize = titleFontSize;
        BodyFontSize = bodyFontSize;
        TextColorHex = textColorHex;
    }

    public Guid? BackgroundAssetId { get; set; }
    public string FontKey { get; set; }
    public int TitleFontSize { get; set; }
    public int BodyFontSize { get; set; }
    public string TextColorHex { get; set; }
}
