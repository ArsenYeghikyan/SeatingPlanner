using EventSeatingPlanner.Application.DTOs.Guests;

namespace EventSeatingPlanner.Application.Interfaces.Services;

public interface IGuestService
{
    Task<IReadOnlyList<GuestDto>> ListAsync(Guid eventId, CancellationToken cancellationToken);
    Task<GuestDto> CreateAsync(Guid eventId, CreateGuestRequest request, CancellationToken cancellationToken);
}
