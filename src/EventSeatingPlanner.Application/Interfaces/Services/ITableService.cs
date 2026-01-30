using EventSeatingPlanner.Application.DTOs.Tables;

namespace EventSeatingPlanner.Application.Interfaces.Services;

public interface ITableService
{
    Task<IReadOnlyList<TableDto>> ListAsync(Guid eventId, CancellationToken cancellationToken);
    Task<TableDto> CreateAsync(Guid eventId, CreateTableRequest request, CancellationToken cancellationToken);
}
