namespace EventSeatingPlanner.Application.DTOs.Tables;

public sealed record CreateTableRequest(
    string Name,
    int Capacity,
    int SortOrder);
