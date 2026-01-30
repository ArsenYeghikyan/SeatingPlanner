using Dapper;
using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using Npgsql;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class PostgresEventRepository(NpgsqlDataSource dataSource) : IEventRepository
{
    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = """
            select id, owner_user_id as OwnerUserId, title, event_date as Date, location, notes
            from events
            where id = @Id;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<Event>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyList<Event>> ListAsync(Guid ownerUserId, CancellationToken cancellationToken)
    {
        const string sql = """
            select id, owner_user_id as OwnerUserId, title, event_date as Date, location, notes
            from events
            where owner_user_id = @OwnerUserId
            order by event_date;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var events = await connection.QueryAsync<Event>(
            new CommandDefinition(sql, new { OwnerUserId = ownerUserId }, cancellationToken: cancellationToken));

        return events.AsList();
    }

    public async Task AddAsync(Event @event, CancellationToken cancellationToken)
    {
        const string sql = """
            insert into events (id, owner_user_id, title, event_date, location, notes)
            values (@Id, @OwnerUserId, @Title, @Date, @Location, @Notes);
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, @event, cancellationToken: cancellationToken));
    }

    public async Task UpdateAsync(Event @event, CancellationToken cancellationToken)
    {
        const string sql = """
            update events
            set owner_user_id = @OwnerUserId,
                title = @Title,
                event_date = @Date,
                location = @Location,
                notes = @Notes
            where id = @Id;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, @event, cancellationToken: cancellationToken));
    }

    public async Task DeleteAsync(Event @event, CancellationToken cancellationToken)
    {
        const string sql = """
            delete from events where id = @Id;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, new { @event.Id }, cancellationToken: cancellationToken));
    }
}
