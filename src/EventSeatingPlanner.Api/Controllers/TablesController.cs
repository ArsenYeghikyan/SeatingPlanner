using EventSeatingPlanner.Application.DTOs.Tables;
using EventSeatingPlanner.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventSeatingPlanner.Api.Controllers;

[ApiController]
[Route("api/events/{eventId:guid}/tables")]
public sealed class TablesController : ControllerBase
{
    private readonly ITableService _tableService;

    public TablesController(ITableService tableService)
    {
        _tableService = tableService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TableDto>>> List(Guid eventId, CancellationToken cancellationToken)
    {
        var tables = await _tableService.ListAsync(eventId, cancellationToken);
        return Ok(tables);
    }

    [HttpPost]
    public async Task<ActionResult<TableDto>> Create(
        Guid eventId,
        [FromBody] CreateTableRequest request,
        CancellationToken cancellationToken)
    {
        var created = await _tableService.CreateAsync(eventId, request, cancellationToken);
        return CreatedAtAction(nameof(List), new { eventId }, created);
    }
}
