namespace EventSeatingPlanner.Application.DTOs.Auth;

public sealed record AuthResponse(
    string Email,
    string Token,
    DateTimeOffset ExpiresAt);
