using Dapper;
using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using Npgsql;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class PostgresTableRepository(NpgsqlDataSource dataSource) : ITableRepository
{
    public async Task<IReadOnlyList<Table>> ListByEventAsync(Guid eventId, CancellationToken cancellationToken)
    {
        const string sql = """
            select id, event_id as EventId, name, capacity, sort_order as SortOrder
            from tables
            where event_id = @EventId
            order by sort_order;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var tables = await connection.QueryAsync<Table>(
            new CommandDefinition(sql, new { EventId = eventId }, cancellationToken: cancellationToken));

        return tables.AsList();
    }

    public async Task AddAsync(Table table, CancellationToken cancellationToken)
    {
        const string sql = """
            insert into tables (id, event_id, name, capacity, sort_order)
            values (@Id, @EventId, @Name, @Capacity, @SortOrder);
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, table, cancellationToken: cancellationToken));
    }

    public async Task UpdateAsync(Table table, CancellationToken cancellationToken)
    {
        const string sql = """
            update tables
            set event_id = @EventId,
                name = @Name,
                capacity = @Capacity,
                sort_order = @SortOrder
            where id = @Id;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, table, cancellationToken: cancellationToken));
    }

    public async Task DeleteAsync(Table table, CancellationToken cancellationToken)
    {
        const string sql = """
            delete from tables where id = @Id;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, new { table.Id }, cancellationToken: cancellationToken));
    }
}
