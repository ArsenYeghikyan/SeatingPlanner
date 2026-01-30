namespace EventSeatingPlanner.App.Models;

public sealed record TableDto(
    Guid Id,
    Guid EventId,
    string Name,
    int Capacity,
    int SortOrder);

public sealed record CreateTableRequest(
    string Name,
    int Capacity,
    int SortOrder);
