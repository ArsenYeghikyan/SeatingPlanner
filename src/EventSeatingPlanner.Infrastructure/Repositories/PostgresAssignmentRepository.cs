using Dapper;
using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using Npgsql;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class PostgresAssignmentRepository(NpgsqlDataSource dataSource) : IAssignmentRepository
{
    public async Task<IReadOnlyList<Assignment>> ListByEventAsync(Guid eventId, CancellationToken cancellationToken)
    {
        const string sql = """
            select id, event_id as EventId, table_id as TableId, guest_id as GuestId, seat_number as SeatNumber
            from assignments
            where event_id = @EventId
            order by seat_number nulls last;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var assignments = await connection.QueryAsync<Assignment>(
            new CommandDefinition(sql, new { EventId = eventId }, cancellationToken: cancellationToken));

        return assignments.AsList();
    }

    public async Task AddAsync(Assignment assignment, CancellationToken cancellationToken)
    {
        const string sql = """
            insert into assignments (id, event_id, table_id, guest_id, seat_number)
            values (@Id, @EventId, @TableId, @GuestId, @SeatNumber);
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, assignment, cancellationToken: cancellationToken));
    }

    public async Task UpdateAsync(Assignment assignment, CancellationToken cancellationToken)
    {
        const string sql = """
            update assignments
            set event_id = @EventId,
                table_id = @TableId,
                guest_id = @GuestId,
                seat_number = @SeatNumber
            where id = @Id;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, assignment, cancellationToken: cancellationToken));
    }

    public async Task DeleteAsync(Assignment assignment, CancellationToken cancellationToken)
    {
        const string sql = """
            delete from assignments where id = @Id;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, new { assignment.Id }, cancellationToken: cancellationToken));
    }
}
