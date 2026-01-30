namespace EventSeatingPlanner.App.Models;

public sealed record AssetDto(
    Guid Id,
    Guid? EventId,
    string Type,
    string FileName,
    string ContentType,
    long SizeBytes);
