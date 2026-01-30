using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class InMemoryEventRepository : IEventRepository
{
    private readonly List<Event> _events = new();

    public Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var @event = _events.FirstOrDefault(item => item.Id == id);
        return Task.FromResult(@event);
    }

    public Task<IReadOnlyList<Event>> ListAsync(Guid ownerUserId, CancellationToken cancellationToken)
    {
        var events = _events.Where(item => item.OwnerUserId == ownerUserId).ToList();
        return Task.FromResult<IReadOnlyList<Event>>(events);
    }

    public Task AddAsync(Event @event, CancellationToken cancellationToken)
    {
        _events.Add(@event);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Event @event, CancellationToken cancellationToken)
    {
        var index = _events.FindIndex(item => item.Id == @event.Id);
        if (index >= 0)
        {
            _events[index] = @event;
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Event @event, CancellationToken cancellationToken)
    {
        _events.RemoveAll(item => item.Id == @event.Id);
        return Task.CompletedTask;
    }
}
