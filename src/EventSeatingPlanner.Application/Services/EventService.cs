using EventSeatingPlanner.Application.DTOs;
using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using EventSeatingPlanner.Application.Interfaces.Services;

namespace EventSeatingPlanner.Application.Services;

public sealed class EventService(
    IEventRepository eventRepository,
    ITableRepository tableRepository,
    IGuestRepository guestRepository) : IEventService
{
    public async Task<IReadOnlyList<EventSummaryDto>> ListAsync(
        Guid ownerUserId,
        CancellationToken cancellationToken)
    {
        var events = await eventRepository.ListAsync(ownerUserId, cancellationToken);
        var summaries = new List<EventSummaryDto>();

        foreach (var @event in events)
        {
            var tables = await tableRepository.ListByEventAsync(@event.Id, cancellationToken);
            var guests = await guestRepository.ListByEventAsync(@event.Id, cancellationToken);

            summaries.Add(new EventSummaryDto(
                @event.Id,
                @event.Title,
                @event.Date,
                @event.Location,
                tables.Count,
                guests.Count));
        }

        return summaries;
    }

    public async Task<EventDetailDto?> GetAsync(
        Guid ownerUserId,
        Guid eventId,
        CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(eventId, cancellationToken);

        if (@event is null || @event.OwnerUserId != ownerUserId)
        {
            return null;
        }

        return new EventDetailDto(
            @event.Id,
            @event.Title,
            @event.Date,
            @event.Location,
            @event.Notes);
    }

    public async Task<EventDetailDto> CreateAsync(
        Guid ownerUserId,
        CreateEventRequest request,
        CancellationToken cancellationToken)
    {
        var @event = new Event
        {
            Id = Guid.NewGuid(),
            OwnerUserId = ownerUserId,
            Title = request.Title,
            Date = request.Date,
            Location = request.Location,
            Notes = request.Notes
        };

        await eventRepository.AddAsync(@event, cancellationToken);

        return new EventDetailDto(
            @event.Id,
            @event.Title,
            @event.Date,
            @event.Location,
            @event.Notes);
    }
}
