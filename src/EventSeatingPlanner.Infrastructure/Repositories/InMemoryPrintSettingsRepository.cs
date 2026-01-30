using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class InMemoryPrintSettingsRepository : IPrintSettingsRepository
{
    private readonly Dictionary<Guid, EventPrintSettings> _settings = new();

    public Task<EventPrintSettings?> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        _settings.TryGetValue(eventId, out var settings);
        return Task.FromResult(settings);
    }

    public Task UpsertAsync(EventPrintSettings settings, CancellationToken cancellationToken)
    {
        _settings[settings.EventId] = settings;
        return Task.CompletedTask;
    }
}
