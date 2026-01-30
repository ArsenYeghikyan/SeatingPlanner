namespace EventSeatingPlanner.Application.DTOs.Guests;

public sealed record CreateGuestRequest(
    string FullName,
    string? Phone,
    string? Email,
    string? Notes,
    string? Category);
