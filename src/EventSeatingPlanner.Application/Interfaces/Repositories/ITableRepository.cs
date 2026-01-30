using EventSeatingPlanner.Application.Entities;

namespace EventSeatingPlanner.Application.Interfaces.Repositories;

public interface ITableRepository
{
    Task<IReadOnlyList<Table>> ListByEventAsync(Guid eventId, CancellationToken cancellationToken);
    Task AddAsync(Table table, CancellationToken cancellationToken);
    Task UpdateAsync(Table table, CancellationToken cancellationToken);
    Task DeleteAsync(Table table, CancellationToken cancellationToken);
}
