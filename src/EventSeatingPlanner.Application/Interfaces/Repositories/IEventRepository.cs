using EventSeatingPlanner.Application.Entities;

namespace EventSeatingPlanner.Application.Interfaces.Repositories;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Event>> ListAsync(Guid ownerUserId, CancellationToken cancellationToken);
    Task AddAsync(Event @event, CancellationToken cancellationToken);
    Task UpdateAsync(Event @event, CancellationToken cancellationToken);
    Task DeleteAsync(Event @event, CancellationToken cancellationToken);
}
