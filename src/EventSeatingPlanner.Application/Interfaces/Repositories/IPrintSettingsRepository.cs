using EventSeatingPlanner.Application.Entities;

namespace EventSeatingPlanner.Application.Interfaces.Repositories;

public interface IPrintSettingsRepository
{
    Task<EventPrintSettings?> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken);
    Task UpsertAsync(EventPrintSettings settings, CancellationToken cancellationToken);
}
