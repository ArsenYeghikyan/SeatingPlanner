using EventSeatingPlanner.Application.DTOs;
using EventSeatingPlanner.Application.DTOs.PrintSettings;

namespace EventSeatingPlanner.Application.Interfaces.Services;

public interface IPrintSettingsService
{
    Task<PrintSettingsDto> GetAsync(Guid eventId, CancellationToken cancellationToken);
    Task<PrintSettingsDto> UpdateAsync(Guid eventId, UpdatePrintSettingsRequest request, CancellationToken cancellationToken);
}
