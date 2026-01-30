var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Repositories.IEventRepository, EventSeatingPlanner.Infrastructure.Repositories.InMemoryEventRepository>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Repositories.ITableRepository, EventSeatingPlanner.Infrastructure.Repositories.InMemoryTableRepository>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Repositories.IGuestRepository, EventSeatingPlanner.Infrastructure.Repositories.InMemoryGuestRepository>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Services.IEventService, EventSeatingPlanner.Application.Services.EventService>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Services.ITableService, EventSeatingPlanner.Application.Services.TableService>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Services.IGuestService, EventSeatingPlanner.Application.Services.GuestService>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();
