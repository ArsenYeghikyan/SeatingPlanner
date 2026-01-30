namespace EventSeatingPlanner.App.Models;

public sealed record AssignmentDto(
    Guid Id,
    Guid EventId,
    Guid TableId,
    Guid GuestId,
    int? SeatNumber);

public sealed class CreateAssignmentRequest
{
    public CreateAssignmentRequest(Guid tableId, Guid guestId, int? seatNumber)
    {
        TableId = tableId;
        GuestId = guestId;
        SeatNumber = seatNumber;
    }

    public Guid TableId { get; set; }
    public Guid GuestId { get; set; }
    public int? SeatNumber { get; set; }
}
