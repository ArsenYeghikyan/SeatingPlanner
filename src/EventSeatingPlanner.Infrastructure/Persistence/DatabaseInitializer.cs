using Npgsql;

namespace EventSeatingPlanner.Infrastructure.Persistence;

public sealed class DatabaseInitializer(NpgsqlDataSource dataSource) : IDatabaseInitializer
{
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS events (
                id uuid PRIMARY KEY,
                owner_user_id uuid NOT NULL,
                title text NOT NULL,
                event_date date NOT NULL,
                location text NULL,
                notes text NULL
            );

            CREATE TABLE IF NOT EXISTS assets (
                id uuid PRIMARY KEY,
                owner_user_id uuid NOT NULL,
                event_id uuid NULL REFERENCES events(id) ON DELETE SET NULL,
                type text NOT NULL,
                file_name text NOT NULL,
                content_type text NOT NULL,
                storage_path text NOT NULL,
                size_bytes bigint NOT NULL
            );

            CREATE TABLE IF NOT EXISTS tables (
                id uuid PRIMARY KEY,
                event_id uuid NOT NULL REFERENCES events(id) ON DELETE CASCADE,
                name text NOT NULL,
                capacity int NOT NULL,
                sort_order int NOT NULL
            );

            CREATE TABLE IF NOT EXISTS guests (
                id uuid PRIMARY KEY,
                event_id uuid NOT NULL REFERENCES events(id) ON DELETE CASCADE,
                full_name text NOT NULL,
                phone text NULL,
                email text NULL,
                notes text NULL,
                category text NULL
            );

            CREATE TABLE IF NOT EXISTS assignments (
                id uuid PRIMARY KEY,
                event_id uuid NOT NULL REFERENCES events(id) ON DELETE CASCADE,
                table_id uuid NOT NULL REFERENCES tables(id) ON DELETE CASCADE,
                guest_id uuid NOT NULL REFERENCES guests(id) ON DELETE CASCADE,
                seat_number int NULL
            );

            CREATE TABLE IF NOT EXISTS print_settings (
                event_id uuid PRIMARY KEY REFERENCES events(id) ON DELETE CASCADE,
                background_asset_id uuid NULL REFERENCES assets(id) ON DELETE SET NULL,
                font_key text NOT NULL,
                title_font_size int NOT NULL,
                body_font_size int NOT NULL,
                text_color_hex text NOT NULL,
                updated_at timestamptz NOT NULL
            );
        ";

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
