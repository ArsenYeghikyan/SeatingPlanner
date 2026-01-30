using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class InMemoryAssignmentRepository : IAssignmentRepository
{
    private readonly List<Assignment> _assignments = new();

    public Task<IReadOnlyList<Assignment>> ListByEventAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var assignments = _assignments.Where(assignment => assignment.EventId == eventId).ToList();
        return Task.FromResult<IReadOnlyList<Assignment>>(assignments);
    }

    public Task AddAsync(Assignment assignment, CancellationToken cancellationToken)
    {
        _assignments.Add(assignment);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Assignment assignment, CancellationToken cancellationToken)
    {
        var index = _assignments.FindIndex(item => item.Id == assignment.Id);
        if (index >= 0)
        {
            _assignments[index] = assignment;
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Assignment assignment, CancellationToken cancellationToken)
    {
        _assignments.RemoveAll(item => item.Id == assignment.Id);
        return Task.CompletedTask;
    }
}
