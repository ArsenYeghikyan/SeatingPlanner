namespace EventSeatingPlanner.Application.DTOs;

public sealed record EventSummaryDto(
    Guid Id,
    string Title,
    DateOnly Date,
    string? Location,
    int TableCount,
    int GuestCount);
