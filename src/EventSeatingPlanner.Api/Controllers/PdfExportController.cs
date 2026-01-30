using EventSeatingPlanner.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventSeatingPlanner.Api.Controllers;

[ApiController]
[Route("api/events/{eventId:guid}/pdf")]
public sealed class PdfExportController : ControllerBase
{
    private readonly IPdfExportService _pdfExportService;

    public PdfExportController(IPdfExportService pdfExportService)
    {
        _pdfExportService = pdfExportService;
    }

    [HttpGet]
    public async Task<IActionResult> Download(Guid eventId, CancellationToken cancellationToken)
    {
        var pdfBytes = await _pdfExportService.ExportSeatingPlanAsync(eventId, cancellationToken);
        var fileName = $"seating-plan-{eventId:N}.pdf";
        return File(pdfBytes, "application/pdf", fileName);
    }
}
