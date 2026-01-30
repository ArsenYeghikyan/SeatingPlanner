namespace EventSeatingPlanner.Application.Interfaces.Services;

public interface IPdfExportService
{
    Task<byte[]> ExportSeatingPlanAsync(Guid eventId, CancellationToken cancellationToken);
}
