namespace EventSeatingPlanner.App.Models;

public sealed record GuestDto(
    Guid Id,
    Guid EventId,
    string FullName,
    string? Phone,
    string? Email,
    string? Notes,
    string? Category);

public sealed record CreateGuestRequest(
    string FullName,
    string? Phone,
    string? Email,
    string? Notes,
    string? Category);
