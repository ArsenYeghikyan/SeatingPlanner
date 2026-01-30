namespace EventSeatingPlanner.Application.DTOs.Assets;

public sealed record AssetDetailDto(
    Guid Id,
    Guid? EventId,
    string Type,
    string FileName,
    string ContentType,
    string StoragePath,
    long SizeBytes);
