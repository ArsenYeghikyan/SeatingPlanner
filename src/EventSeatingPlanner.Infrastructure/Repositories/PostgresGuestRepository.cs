using Dapper;
using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using Npgsql;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class PostgresGuestRepository(NpgsqlDataSource dataSource) : IGuestRepository
{
    public async Task<IReadOnlyList<Guest>> ListByEventAsync(Guid eventId, CancellationToken cancellationToken)
    {
        const string sql = """
            select id, event_id as EventId, full_name as FullName, phone, email, notes, category
            from guests
            where event_id = @EventId
            order by full_name;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var guests = await connection.QueryAsync<Guest>(
            new CommandDefinition(sql, new { EventId = eventId }, cancellationToken: cancellationToken));

        return guests.AsList();
    }

    public async Task AddAsync(Guest guest, CancellationToken cancellationToken)
    {
        const string sql = """
            insert into guests (id, event_id, full_name, phone, email, notes, category)
            values (@Id, @EventId, @FullName, @Phone, @Email, @Notes, @Category);
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, guest, cancellationToken: cancellationToken));
    }

    public async Task UpdateAsync(Guest guest, CancellationToken cancellationToken)
    {
        const string sql = """
            update guests
            set event_id = @EventId,
                full_name = @FullName,
                phone = @Phone,
                email = @Email,
                notes = @Notes,
                category = @Category
            where id = @Id;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, guest, cancellationToken: cancellationToken));
    }

    public async Task DeleteAsync(Guest guest, CancellationToken cancellationToken)
    {
        const string sql = """
            delete from guests where id = @Id;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, new { guest.Id }, cancellationToken: cancellationToken));
    }
}
