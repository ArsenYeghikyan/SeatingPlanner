using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using EventSeatingPlanner.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class PostgresEventRepository(ApplicationDbContext dbContext) : IEventRepository
{
    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Events
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Event>> ListAsync(Guid ownerUserId, CancellationToken cancellationToken)
    {
        return await dbContext.Events
            .AsNoTracking()
            .Where(e => e.OwnerUserId == ownerUserId)
            .OrderBy(e => e.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Event @event, CancellationToken cancellationToken)
    {
        dbContext.Events.Add(@event);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Event @event, CancellationToken cancellationToken)
    {
        dbContext.Events.Update(@event);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Event @event, CancellationToken cancellationToken)
    {
        dbContext.Events.Remove(@event);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
