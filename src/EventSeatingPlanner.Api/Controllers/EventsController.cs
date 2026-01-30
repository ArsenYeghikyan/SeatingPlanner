using EventSeatingPlanner.Application.DTOs;
using EventSeatingPlanner.Api.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventSeatingPlanner.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/events")]
public sealed class EventsController : ControllerBase
{
    private readonly EventSeatingPlanner.Application.Interfaces.Services.IEventService _eventService;

    public EventsController(EventSeatingPlanner.Application.Interfaces.Services.IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EventSummaryDto>>> List(CancellationToken cancellationToken)
    {
        var events = await _eventService.ListAsync(User.GetUserId(), cancellationToken);
        return Ok(events);
    }

    [HttpGet("{eventId:guid}")]
    public async Task<ActionResult<EventDetailDto>> Get(Guid eventId, CancellationToken cancellationToken)
    {
        var @event = await _eventService.GetAsync(User.GetUserId(), eventId, cancellationToken);
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
        var created = await _eventService.CreateAsync(User.GetUserId(), request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { eventId = created.Id }, created);
    }
}
