namespace EventSeatingPlanner.Application.DTOs;

public sealed record CreateEventRequest(
    string Title,
    DateOnly Date,
    string? Location,
    string? Notes);
