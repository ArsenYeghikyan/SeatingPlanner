using EventSeatingPlanner.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<EventSeatingPlanner.Application.Interfaces.Services.IEventService, EventSeatingPlanner.Application.Services.EventService>();
builder.Services.AddScoped<EventSeatingPlanner.Application.Interfaces.Services.ITableService, EventSeatingPlanner.Application.Services.TableService>();
builder.Services.AddScoped<EventSeatingPlanner.Application.Interfaces.Services.IGuestService, EventSeatingPlanner.Application.Services.GuestService>();
builder.Services.AddScoped<EventSeatingPlanner.Application.Interfaces.Services.IAssignmentService, EventSeatingPlanner.Application.Services.AssignmentService>();
builder.Services.AddScoped<EventSeatingPlanner.Application.Interfaces.Services.IPrintSettingsService, EventSeatingPlanner.Application.Services.PrintSettingsService>();
builder.Services.AddScoped<EventSeatingPlanner.Application.Interfaces.Services.IPdfExportService, EventSeatingPlanner.Infrastructure.Services.PdfExportService>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Services.IAssetStorage, EventSeatingPlanner.Infrastructure.Storage.InMemoryAssetStorage>();
builder.Services.AddScoped<EventSeatingPlanner.Application.Interfaces.Services.IAssetService, EventSeatingPlanner.Application.Services.AssetService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EventSeatingPlanner.Infrastructure.Persistence.ApplicationDbContext>();
    await dbContext.Database.EnsureCreatedAsync(CancellationToken.None);
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();
