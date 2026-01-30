namespace EventSeatingPlanner.Application.Entities;

public sealed class Asset
{
    public Guid Id { get; set; }
    public Guid OwnerUserId { get; set; }
    public Guid? EventId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
}
