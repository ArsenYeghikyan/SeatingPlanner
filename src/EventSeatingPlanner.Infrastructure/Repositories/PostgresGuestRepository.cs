using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using EventSeatingPlanner.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class PostgresGuestRepository(ApplicationDbContext dbContext) : IGuestRepository
{
    public async Task<IReadOnlyList<Guest>> ListByEventAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return await dbContext.Guests
            .AsNoTracking()
            .Where(g => g.EventId == eventId)
            .OrderBy(g => g.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Guest guest, CancellationToken cancellationToken)
    {
        dbContext.Guests.Add(guest);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Guest guest, CancellationToken cancellationToken)
    {
        dbContext.Guests.Update(guest);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guest guest, CancellationToken cancellationToken)
    {
        dbContext.Guests.Remove(guest);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
