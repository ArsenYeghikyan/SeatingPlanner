namespace EventSeatingPlanner.App.Models;

public sealed record EventSummaryDto(
    Guid Id,
    string Title,
    DateOnly Date,
    string? Location,
    int TableCount,
    int GuestCount);

public sealed record EventDetailDto(
    Guid Id,
    string Title,
    DateOnly Date,
    string? Location,
    string? Notes);

public sealed class CreateEventRequest
{
    public CreateEventRequest(string title, DateOnly date, string? location, string? notes)
    {
        Title = title;
        Date = date;
        Location = location;
        Notes = notes;
    }

    public string Title { get; set; }
    public DateOnly Date { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
}
