namespace EventSeatingPlanner.Application.Entities;

public sealed class Event
{
    public Guid Id { get; set; }
    public Guid OwnerUserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
}
