using EventSeatingPlanner.Application.DTOs.Guests;
using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using EventSeatingPlanner.Application.Interfaces.Services;

namespace EventSeatingPlanner.Application.Services;

public sealed class GuestService(IGuestRepository guestRepository) : IGuestService
{
    public async Task<IReadOnlyList<GuestDto>> ListAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var guests = await guestRepository.ListByEventAsync(eventId, cancellationToken);

        return guests
            .Select(guest => new GuestDto(
                guest.Id,
                guest.EventId,
                guest.FullName,
                guest.Phone,
                guest.Email,
                guest.Notes,
                guest.Category))
            .ToList();
    }

    public async Task<GuestDto> CreateAsync(Guid eventId, CreateGuestRequest request, CancellationToken cancellationToken)
    {
        var guest = new Guest
        {
            Id = Guid.NewGuid(),
            EventId = eventId,
            FullName = request.FullName,
            Phone = request.Phone,
            Email = request.Email,
            Notes = request.Notes,
            Category = request.Category
        };

        await guestRepository.AddAsync(guest, cancellationToken);

        return new GuestDto(
            guest.Id,
            guest.EventId,
            guest.FullName,
            guest.Phone,
            guest.Email,
            guest.Notes,
            guest.Category);
    }
}
