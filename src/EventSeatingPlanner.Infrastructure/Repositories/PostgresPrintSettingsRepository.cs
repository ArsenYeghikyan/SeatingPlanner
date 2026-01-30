using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using EventSeatingPlanner.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class PostgresPrintSettingsRepository(ApplicationDbContext dbContext) : IPrintSettingsRepository
{
    public async Task<EventPrintSettings?> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return await dbContext.PrintSettings
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.EventId == eventId, cancellationToken);
    }

    public async Task UpsertAsync(EventPrintSettings settings, CancellationToken cancellationToken)
    {
        var existing = await dbContext.PrintSettings
            .SingleOrDefaultAsync(p => p.EventId == settings.EventId, cancellationToken);

        if (existing is null)
        {
            dbContext.PrintSettings.Add(settings);
        }
        else
        {
            dbContext.Entry(existing).CurrentValues.SetValues(settings);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
