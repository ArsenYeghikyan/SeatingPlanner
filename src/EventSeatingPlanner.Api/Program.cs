var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Repositories.IEventRepository, EventSeatingPlanner.Infrastructure.Repositories.InMemoryEventRepository>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Services.IEventService, EventSeatingPlanner.Application.Services.EventService>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();
