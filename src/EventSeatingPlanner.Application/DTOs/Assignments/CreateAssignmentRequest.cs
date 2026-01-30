namespace EventSeatingPlanner.Application.DTOs.Assignments;

public sealed record CreateAssignmentRequest(
    Guid TableId,
    Guid GuestId,
    int? SeatNumber);
