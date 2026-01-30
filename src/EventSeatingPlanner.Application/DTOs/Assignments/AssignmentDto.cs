namespace EventSeatingPlanner.Application.DTOs.Assignments;

public sealed record AssignmentDto(
    Guid Id,
    Guid EventId,
    Guid TableId,
    Guid GuestId,
    int? SeatNumber);
