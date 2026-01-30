namespace EventSeatingPlanner.Application.DTOs.Guests;

public sealed record GuestDto(
    Guid Id,
    Guid EventId,
    string FullName,
    string? Phone,
    string? Email,
    string? Notes,
    string? Category);
