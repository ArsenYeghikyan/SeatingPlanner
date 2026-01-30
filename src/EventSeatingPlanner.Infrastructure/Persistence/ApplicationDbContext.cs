using EventSeatingPlanner.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventSeatingPlanner.Infrastructure.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Table> Tables => Set<Table>();
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<EventPrintSettings> PrintSettings => Set<EventPrintSettings>();
    public DbSet<Asset> Assets => Set<Asset>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).HasColumnName("email");
            entity.Property(u => u.PasswordHash).HasColumnName("password_hash");
            entity.Property(u => u.CreatedAt).HasColumnName("created_at");
            entity.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("events");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OwnerUserId).HasColumnName("owner_user_id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Date).HasColumnName("event_date").HasColumnType("date");
            entity.Property(e => e.Location).HasColumnName("location");
            entity.Property(e => e.Notes).HasColumnName("notes");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.ToTable("tables");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.EventId).HasColumnName("event_id");
            entity.Property(t => t.Name).HasColumnName("name");
            entity.Property(t => t.Capacity).HasColumnName("capacity");
            entity.Property(t => t.SortOrder).HasColumnName("sort_order");
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.ToTable("guests");
            entity.HasKey(g => g.Id);
            entity.Property(g => g.EventId).HasColumnName("event_id");
            entity.Property(g => g.FullName).HasColumnName("full_name");
            entity.Property(g => g.Phone).HasColumnName("phone");
            entity.Property(g => g.Email).HasColumnName("email");
            entity.Property(g => g.Notes).HasColumnName("notes");
            entity.Property(g => g.Category).HasColumnName("category");
        });

        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.ToTable("assignments");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.EventId).HasColumnName("event_id");
            entity.Property(a => a.TableId).HasColumnName("table_id");
            entity.Property(a => a.GuestId).HasColumnName("guest_id");
            entity.Property(a => a.SeatNumber).HasColumnName("seat_number");
        });

        modelBuilder.Entity<EventPrintSettings>(entity =>
        {
            entity.ToTable("print_settings");
            entity.HasKey(p => p.EventId);
            entity.Property(p => p.EventId).HasColumnName("event_id");
            entity.Property(p => p.BackgroundAssetId).HasColumnName("background_asset_id");
            entity.Property(p => p.FontKey).HasColumnName("font_key");
            entity.Property(p => p.TitleFontSize).HasColumnName("title_font_size");
            entity.Property(p => p.BodyFontSize).HasColumnName("body_font_size");
            entity.Property(p => p.TextColorHex).HasColumnName("text_color_hex");
            entity.Property(p => p.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.ToTable("assets");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.OwnerUserId).HasColumnName("owner_user_id");
            entity.Property(a => a.EventId).HasColumnName("event_id");
            entity.Property(a => a.Type).HasColumnName("type");
            entity.Property(a => a.FileName).HasColumnName("file_name");
            entity.Property(a => a.ContentType).HasColumnName("content_type");
            entity.Property(a => a.StoragePath).HasColumnName("storage_path");
            entity.Property(a => a.SizeBytes).HasColumnName("size_bytes");
        });
    }
}
