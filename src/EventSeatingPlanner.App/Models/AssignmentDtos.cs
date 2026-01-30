namespace EventSeatingPlanner.App.Models;

public sealed record AssignmentDto(
    Guid Id,
    Guid EventId,
    Guid TableId,
    Guid GuestId,
    int? SeatNumber);

public sealed record CreateAssignmentRequest(
    Guid TableId,
    Guid GuestId,
    int? SeatNumber);
