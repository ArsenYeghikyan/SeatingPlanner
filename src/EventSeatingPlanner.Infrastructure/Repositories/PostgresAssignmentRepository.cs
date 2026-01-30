using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using EventSeatingPlanner.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class PostgresAssignmentRepository(ApplicationDbContext dbContext) : IAssignmentRepository
{
    public async Task<IReadOnlyList<Assignment>> ListByEventAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return await dbContext.Assignments
            .AsNoTracking()
            .Where(a => a.EventId == eventId)
            .OrderBy(a => a.SeatNumber == null)
            .ThenBy(a => a.SeatNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Assignment assignment, CancellationToken cancellationToken)
    {
        dbContext.Assignments.Add(assignment);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Assignment assignment, CancellationToken cancellationToken)
    {
        dbContext.Assignments.Update(assignment);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Assignment assignment, CancellationToken cancellationToken)
    {
        dbContext.Assignments.Remove(assignment);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
