using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using EventSeatingPlanner.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class PostgresTableRepository(ApplicationDbContext dbContext) : ITableRepository
{
    public async Task<IReadOnlyList<Table>> ListByEventAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return await dbContext.Tables
            .AsNoTracking()
            .Where(t => t.EventId == eventId)
            .OrderBy(t => t.SortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Table table, CancellationToken cancellationToken)
    {
        dbContext.Tables.Add(table);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Table table, CancellationToken cancellationToken)
    {
        dbContext.Tables.Update(table);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Table table, CancellationToken cancellationToken)
    {
        dbContext.Tables.Remove(table);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
