namespace EventSeatingPlanner.App.Models;

public sealed record RegisterRequest(
    string Email,
    string Password,
    string? FullName);

public sealed record LoginRequest(
    string Email,
    string Password);

public sealed record AuthResponse(
    string Email,
    string Token,
    DateTimeOffset ExpiresAt);
