namespace EventSeatingPlanner.Application.Entities;

public sealed class Assignment
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid TableId { get; set; }
    public Guid GuestId { get; set; }
    public int? SeatNumber { get; set; }
}
