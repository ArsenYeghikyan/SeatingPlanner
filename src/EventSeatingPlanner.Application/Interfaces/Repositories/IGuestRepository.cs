using EventSeatingPlanner.Application.Entities;

namespace EventSeatingPlanner.Application.Interfaces.Repositories;

public interface IGuestRepository
{
    Task<IReadOnlyList<Guest>> ListByEventAsync(Guid eventId, CancellationToken cancellationToken);
    Task AddAsync(Guest guest, CancellationToken cancellationToken);
    Task UpdateAsync(Guest guest, CancellationToken cancellationToken);
    Task DeleteAsync(Guest guest, CancellationToken cancellationToken);
}
