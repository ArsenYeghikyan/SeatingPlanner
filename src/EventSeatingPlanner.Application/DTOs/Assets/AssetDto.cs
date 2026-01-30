namespace EventSeatingPlanner.Application.DTOs.Assets;

public sealed record AssetDto(
    Guid Id,
    Guid? EventId,
    string Type,
    string FileName,
    string ContentType,
    long SizeBytes);
