using EventSeatingPlanner.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EventSeatingPlanner.Api.Controllers;

[ApiController]
[Route("api/events")]
public sealed class EventsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyList<EventSummaryDto>> List()
    {
        var sample = new EventSummaryDto(
            Guid.Empty,
            "Demo Event",
            DateOnly.FromDateTime(DateTime.UtcNow),
            "Online",
            0,
            0);

        return Ok(new[] { sample });
    }
}
