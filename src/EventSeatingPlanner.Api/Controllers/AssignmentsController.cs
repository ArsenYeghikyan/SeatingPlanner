using EventSeatingPlanner.Application.DTOs.Assignments;
using EventSeatingPlanner.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventSeatingPlanner.Api.Controllers;

[ApiController]
[Route("api/events/{eventId:guid}/assignments")]
public sealed class AssignmentsController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentsController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AssignmentDto>>> List(Guid eventId, CancellationToken cancellationToken)
    {
        var assignments = await _assignmentService.ListAsync(eventId, cancellationToken);
        return Ok(assignments);
    }

    [HttpPost]
    public async Task<ActionResult<AssignmentDto>> Create(
        Guid eventId,
        [FromBody] CreateAssignmentRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var created = await _assignmentService.CreateAsync(eventId, request, cancellationToken);
            return CreatedAtAction(nameof(List), new { eventId }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{assignmentId:guid}")]
    public async Task<IActionResult> Delete(Guid eventId, Guid assignmentId, CancellationToken cancellationToken)
    {
        try
        {
            await _assignmentService.DeleteAsync(eventId, assignmentId, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
