using EventSeatingPlanner.Application.DTOs;
using EventSeatingPlanner.Application.DTOs.PrintSettings;
using EventSeatingPlanner.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventSeatingPlanner.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/events/{eventId:guid}/print-settings")]
public sealed class PrintSettingsController : ControllerBase
{
    private readonly IPrintSettingsService _printSettingsService;

    public PrintSettingsController(IPrintSettingsService printSettingsService)
    {
        _printSettingsService = printSettingsService;
    }

    [HttpGet]
    public async Task<ActionResult<PrintSettingsDto>> Get(Guid eventId, CancellationToken cancellationToken)
    {
        var settings = await _printSettingsService.GetAsync(eventId, cancellationToken);
        return Ok(settings);
    }

    [HttpPut]
    public async Task<ActionResult<PrintSettingsDto>> Update(
        Guid eventId,
        [FromBody] UpdatePrintSettingsRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await _printSettingsService.UpdateAsync(eventId, request, cancellationToken);
        return Ok(updated);
    }
}
