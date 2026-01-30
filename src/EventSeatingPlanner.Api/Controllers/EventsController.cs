using EventSeatingPlanner.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EventSeatingPlanner.Api.Controllers;

[ApiController]
[Route("api/events")]
public sealed class EventsController : ControllerBase
{
    private static readonly Guid DemoOwnerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private readonly EventSeatingPlanner.Application.Interfaces.Services.IEventService _eventService;

    public EventsController(EventSeatingPlanner.Application.Interfaces.Services.IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EventSummaryDto>>> List(CancellationToken cancellationToken)
    {
        var events = await _eventService.ListAsync(DemoOwnerId, cancellationToken);
        return Ok(events);
    }

    [HttpGet("{eventId:guid}")]
    public async Task<ActionResult<EventDetailDto>> Get(Guid eventId, CancellationToken cancellationToken)
    {
        var @event = await _eventService.GetAsync(DemoOwnerId, eventId, cancellationToken);
        if (@event is null)
        {
            return NotFound();
        }

        return Ok(@event);
    }

    [HttpPost]
    public async Task<ActionResult<EventDetailDto>> Create(
        [FromBody] CreateEventRequest request,
        CancellationToken cancellationToken)
    {
        var created = await _eventService.CreateAsync(DemoOwnerId, request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { eventId = created.Id }, created);
    }
}
