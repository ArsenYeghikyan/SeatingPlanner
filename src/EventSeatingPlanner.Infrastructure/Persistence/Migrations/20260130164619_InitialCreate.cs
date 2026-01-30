using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventSeatingPlanner.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: true),
                    type = table.Column<string>(type: "text", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    content_type = table.Column<string>(type: "text", nullable: false),
                    storage_path = table.Column<string>(type: "text", nullable: false),
                    size_bytes = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    table_id = table.Column<Guid>(type: "uuid", nullable: false),
                    guest_id = table.Column<Guid>(type: "uuid", nullable: false),
                    seat_number = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    event_date = table.Column<DateOnly>(type: "date", nullable: false),
                    location = table.Column<string>(type: "text", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "guests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "print_settings",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    background_asset_id = table.Column<Guid>(type: "uuid", nullable: true),
                    font_key = table.Column<string>(type: "text", nullable: false),
                    title_font_size = table.Column<int>(type: "integer", nullable: false),
                    body_font_size = table.Column<int>(type: "integer", nullable: false),
                    text_color_hex = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_print_settings", x => x.event_id);
                });

            migrationBuilder.CreateTable(
                name: "tables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tables", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assets");

            migrationBuilder.DropTable(
                name: "assignments");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "guests");

            migrationBuilder.DropTable(
                name: "print_settings");

            migrationBuilder.DropTable(
                name: "tables");
        }
    }
}
