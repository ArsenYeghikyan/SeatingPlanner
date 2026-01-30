namespace EventSeatingPlanner.Application.Entities;

public sealed class EventPrintSettings
{
    public Guid EventId { get; set; }
    public Guid? BackgroundAssetId { get; set; }
    public string FontKey { get; set; } = string.Empty;
    public int TitleFontSize { get; set; }
    public int BodyFontSize { get; set; }
    public string TextColorHex { get; set; } = "#000000";
    public DateTimeOffset UpdatedAt { get; set; }
}
