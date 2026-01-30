using Dapper;
using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using Npgsql;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class PostgresAssetRepository(NpgsqlDataSource dataSource) : IAssetRepository
{
    public async Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = """
            select id,
                   owner_user_id as OwnerUserId,
                   event_id as EventId,
                   type,
                   file_name as FileName,
                   content_type as ContentType,
                   storage_path as StoragePath,
                   size_bytes as SizeBytes
            from assets
            where id = @Id;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<Asset>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyList<Asset>> ListByOwnerAsync(Guid ownerUserId, CancellationToken cancellationToken)
    {
        const string sql = """
            select id,
                   owner_user_id as OwnerUserId,
                   event_id as EventId,
                   type,
                   file_name as FileName,
                   content_type as ContentType,
                   storage_path as StoragePath,
                   size_bytes as SizeBytes
            from assets
            where owner_user_id = @OwnerUserId
            order by file_name;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var assets = await connection.QueryAsync<Asset>(
            new CommandDefinition(sql, new { OwnerUserId = ownerUserId }, cancellationToken: cancellationToken));

        return assets.AsList();
    }

    public async Task AddAsync(Asset asset, CancellationToken cancellationToken)
    {
        const string sql = """
            insert into assets (
                id,
                owner_user_id,
                event_id,
                type,
                file_name,
                content_type,
                storage_path,
                size_bytes
            ) values (
                @Id,
                @OwnerUserId,
                @EventId,
                @Type,
                @FileName,
                @ContentType,
                @StoragePath,
                @SizeBytes
            );
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, asset, cancellationToken: cancellationToken));
    }

    public async Task DeleteAsync(Asset asset, CancellationToken cancellationToken)
    {
        const string sql = """
            delete from assets where id = @Id;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, new { asset.Id }, cancellationToken: cancellationToken));
    }
}
