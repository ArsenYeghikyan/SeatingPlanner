using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class InMemoryTableRepository : ITableRepository
{
    private readonly List<Table> _tables = new();

    public Task<IReadOnlyList<Table>> ListByEventAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var tables = _tables.Where(table => table.EventId == eventId).ToList();
        return Task.FromResult<IReadOnlyList<Table>>(tables);
    }

    public Task AddAsync(Table table, CancellationToken cancellationToken)
    {
        _tables.Add(table);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Table table, CancellationToken cancellationToken)
    {
        var index = _tables.FindIndex(item => item.Id == table.Id);
        if (index >= 0)
        {
            _tables[index] = table;
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Table table, CancellationToken cancellationToken)
    {
        _tables.RemoveAll(item => item.Id == table.Id);
        return Task.CompletedTask;
    }
}
