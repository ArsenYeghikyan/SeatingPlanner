using Dapper;
using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using Npgsql;

namespace EventSeatingPlanner.Infrastructure.Repositories;

public sealed class PostgresPrintSettingsRepository(NpgsqlDataSource dataSource) : IPrintSettingsRepository
{
    public async Task<EventPrintSettings?> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        const string sql = """
            select event_id as EventId,
                   background_asset_id as BackgroundAssetId,
                   font_key as FontKey,
                   title_font_size as TitleFontSize,
                   body_font_size as BodyFontSize,
                   text_color_hex as TextColorHex,
                   updated_at as UpdatedAt
            from print_settings
            where event_id = @EventId;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<EventPrintSettings>(
            new CommandDefinition(sql, new { EventId = eventId }, cancellationToken: cancellationToken));
    }

    public async Task UpsertAsync(EventPrintSettings settings, CancellationToken cancellationToken)
    {
        const string sql = """
            insert into print_settings (
                event_id,
                background_asset_id,
                font_key,
                title_font_size,
                body_font_size,
                text_color_hex,
                updated_at
            ) values (
                @EventId,
                @BackgroundAssetId,
                @FontKey,
                @TitleFontSize,
                @BodyFontSize,
                @TextColorHex,
                @UpdatedAt
            )
            on conflict (event_id) do update
            set background_asset_id = excluded.background_asset_id,
                font_key = excluded.font_key,
                title_font_size = excluded.title_font_size,
                body_font_size = excluded.body_font_size,
                text_color_hex = excluded.text_color_hex,
                updated_at = excluded.updated_at;
            """;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, settings, cancellationToken: cancellationToken));
    }
}
