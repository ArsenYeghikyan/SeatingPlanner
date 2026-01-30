using EventSeatingPlanner.Application.DTOs.Tables;
using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using EventSeatingPlanner.Application.Interfaces.Services;

namespace EventSeatingPlanner.Application.Services;

public sealed class TableService(ITableRepository tableRepository) : ITableService
{
    public async Task<IReadOnlyList<TableDto>> ListAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var tables = await tableRepository.ListByEventAsync(eventId, cancellationToken);

        return tables
            .Select(table => new TableDto(
                table.Id,
                table.EventId,
                table.Name,
                table.Capacity,
                table.SortOrder))
            .ToList();
    }

    public async Task<TableDto> CreateAsync(Guid eventId, CreateTableRequest request, CancellationToken cancellationToken)
    {
        var table = new Table
        {
            Id = Guid.NewGuid(),
            EventId = eventId,
            Name = request.Name,
            Capacity = request.Capacity,
            SortOrder = request.SortOrder
        };

        await tableRepository.AddAsync(table, cancellationToken);

        return new TableDto(
            table.Id,
            table.EventId,
            table.Name,
            table.Capacity,
            table.SortOrder);
    }
}
