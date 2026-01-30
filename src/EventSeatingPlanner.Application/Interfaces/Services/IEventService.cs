using EventSeatingPlanner.Application.DTOs;

namespace EventSeatingPlanner.Application.Interfaces.Services;

public interface IEventService
{
    Task<IReadOnlyList<EventSummaryDto>> ListAsync(Guid ownerUserId, CancellationToken cancellationToken);
    Task<EventDetailDto?> GetAsync(Guid ownerUserId, Guid eventId, CancellationToken cancellationToken);
    Task<EventDetailDto> CreateAsync(Guid ownerUserId, CreateEventRequest request, CancellationToken cancellationToken);
}
