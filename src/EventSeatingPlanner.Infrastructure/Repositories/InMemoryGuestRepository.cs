using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class InMemoryGuestRepository : IGuestRepository
{
    private readonly List<Guest> _guests = new();

    public Task<IReadOnlyList<Guest>> ListByEventAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var guests = _guests.Where(guest => guest.EventId == eventId).ToList();
        return Task.FromResult<IReadOnlyList<Guest>>(guests);
    }

    public Task AddAsync(Guest guest, CancellationToken cancellationToken)
    {
        _guests.Add(guest);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Guest guest, CancellationToken cancellationToken)
    {
        var index = _guests.FindIndex(item => item.Id == guest.Id);
        if (index >= 0)
        {
            _guests[index] = guest;
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guest guest, CancellationToken cancellationToken)
    {
        _guests.RemoveAll(item => item.Id == guest.Id);
        return Task.CompletedTask;
    }
}
