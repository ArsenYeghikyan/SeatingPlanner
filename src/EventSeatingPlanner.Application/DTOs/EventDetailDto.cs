namespace EventSeatingPlanner.Application.DTOs;

public sealed record EventDetailDto(
    Guid Id,
    string Title,
    DateOnly Date,
    string? Location,
    string? Notes);
