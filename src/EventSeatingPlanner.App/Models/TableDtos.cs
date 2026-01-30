namespace EventSeatingPlanner.App.Models;

public sealed record TableDto(
    Guid Id,
    Guid EventId,
    string Name,
    int Capacity,
    int SortOrder);

public sealed class CreateTableRequest
{
    public CreateTableRequest(string name, int capacity, int sortOrder)
    {
        Name = name;
        Capacity = capacity;
        SortOrder = sortOrder;
    }

    public string Name { get; set; }
    public int Capacity { get; set; }
    public int SortOrder { get; set; }
}
