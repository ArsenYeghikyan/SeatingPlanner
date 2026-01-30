using EventSeatingPlanner.Application.DTOs.Guests;
using EventSeatingPlanner.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventSeatingPlanner.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/events/{eventId:guid}/guests")]
public sealed class GuestsController : ControllerBase
{
    private readonly IGuestService _guestService;

    public GuestsController(IGuestService guestService)
    {
        _guestService = guestService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GuestDto>>> List(Guid eventId, CancellationToken cancellationToken)
    {
        var guests = await _guestService.ListAsync(eventId, cancellationToken);
        return Ok(guests);
    }

    [HttpPost]
    public async Task<ActionResult<GuestDto>> Create(
        Guid eventId,
        [FromBody] CreateGuestRequest request,
        CancellationToken cancellationToken)
    {
        var created = await _guestService.CreateAsync(eventId, request, cancellationToken);
        return CreatedAtAction(nameof(List), new { eventId }, created);
    }
}
