using EventSeatingPlanner.Application.Entities;

namespace EventSeatingPlanner.Application.Interfaces.Repositories;

public interface IAssignmentRepository
{
    Task<IReadOnlyList<Assignment>> ListByEventAsync(Guid eventId, CancellationToken cancellationToken);
    Task AddAsync(Assignment assignment, CancellationToken cancellationToken);
    Task UpdateAsync(Assignment assignment, CancellationToken cancellationToken);
    Task DeleteAsync(Assignment assignment, CancellationToken cancellationToken);
}
