namespace EventSeatingPlanner.App.Models;

public sealed record EventSummaryDto(
    Guid Id,
    string Title,
    DateOnly Date,
    string? Location,
    int TableCount,
    int GuestCount);

public sealed record EventDetailDto(
    Guid Id,
    string Title,
    DateOnly Date,
    string? Location,
    string? Notes);

public sealed record CreateEventRequest(
    string Title,
    DateOnly Date,
    string? Location,
    string? Notes);
