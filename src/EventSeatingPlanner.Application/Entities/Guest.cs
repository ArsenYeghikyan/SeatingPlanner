namespace EventSeatingPlanner.Application.Entities;

public sealed class Guest
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Notes { get; set; }
    public string? Category { get; set; }
}
