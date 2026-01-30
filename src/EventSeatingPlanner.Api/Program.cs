using EventSeatingPlanner.Infrastructure;
using EventSeatingPlanner.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Event Seating Planner API",
        Version = "v1"
    });
});

// Infrastructure (DbContext, Repos, Dapper handlers, Storage etc.)
builder.Services.AddInfrastructure(builder.Configuration);

// Application services
builder.Services.AddScoped<EventSeatingPlanner.Application.Interfaces.Services.IEventService, EventSeatingPlanner.Application.Services.EventService>();
builder.Services.AddScoped<EventSeatingPlanner.Application.Interfaces.Services.ITableService, EventSeatingPlanner.Application.Services.TableService>();
builder.Services.AddScoped<EventSeatingPlanner.Application.Interfaces.Services.IGuestService, EventSeatingPlanner.Application.Services.GuestService>();
builder.Services.AddScoped<EventSeatingPlanner.Application.Interfaces.Services.IAssignmentService, EventSeatingPlanner.Application.Services.AssignmentService>();
builder.Services.AddScoped<EventSeatingPlanner.Application.Interfaces.Services.IPrintSettingsService, EventSeatingPlanner.Application.Services.PrintSettingsService>();
builder.Services.AddScoped<EventSeatingPlanner.Application.Interfaces.Services.IAssetService, EventSeatingPlanner.Application.Services.AssetService>();

// PDF export (если реализаци€ в Infrastructure Ч ок)
builder.Services.AddScoped<EventSeatingPlanner.Application.Interfaces.Services.IPdfExportService, EventSeatingPlanner.Infrastructure.Services.PdfExportService>();

// Storage (дл€ MVP ok, потом заменить на файловое/S3)
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Services.IAssetStorage, EventSeatingPlanner.Infrastructure.Storage.InMemoryAssetStorage>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Seating Planner API v1");
    });
}

app.UseHttpsRedirection();

// если добавишь auth Ч будет готово
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // –екомендовано вместо EnsureCreated (особенно если будут миграции)
    await db.Database.MigrateAsync();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();

