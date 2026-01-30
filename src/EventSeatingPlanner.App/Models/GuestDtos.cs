namespace EventSeatingPlanner.App.Models;

public sealed record GuestDto(
    Guid Id,
    Guid EventId,
    string FullName,
    string? Phone,
    string? Email,
    string? Notes,
    string? Category);

public sealed class CreateGuestRequest
{
    public CreateGuestRequest(string fullName, string? phone, string? email, string? notes, string? category)
    {
        FullName = fullName;
        Phone = phone;
        Email = email;
        Notes = notes;
        Category = category;
    }

    public string FullName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Notes { get; set; }
    public string? Category { get; set; }
}
