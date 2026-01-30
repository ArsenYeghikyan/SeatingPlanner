namespace EventSeatingPlanner.Application.DTOs.Tables;

public sealed record TableDto(
    Guid Id,
    Guid EventId,
    string Name,
    int Capacity,
    int SortOrder);
