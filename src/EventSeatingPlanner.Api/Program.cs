var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        var jwtSection = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(jwtSection["Key"] ?? string.Empty))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Repositories.IEventRepository, EventSeatingPlanner.Infrastructure.Repositories.InMemoryEventRepository>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Repositories.ITableRepository, EventSeatingPlanner.Infrastructure.Repositories.InMemoryTableRepository>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Repositories.IGuestRepository, EventSeatingPlanner.Infrastructure.Repositories.InMemoryGuestRepository>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Repositories.IAssignmentRepository, EventSeatingPlanner.Infrastructure.Repositories.InMemoryAssignmentRepository>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Repositories.IPrintSettingsRepository, EventSeatingPlanner.Infrastructure.Repositories.InMemoryPrintSettingsRepository>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Repositories.IAssetRepository, EventSeatingPlanner.Infrastructure.Repositories.InMemoryAssetRepository>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Repositories.IUserRepository, EventSeatingPlanner.Infrastructure.Repositories.InMemoryUserRepository>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Services.IEventService, EventSeatingPlanner.Application.Services.EventService>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Services.ITableService, EventSeatingPlanner.Application.Services.TableService>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Services.IGuestService, EventSeatingPlanner.Application.Services.GuestService>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Services.IAssignmentService, EventSeatingPlanner.Application.Services.AssignmentService>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Services.IPrintSettingsService, EventSeatingPlanner.Application.Services.PrintSettingsService>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Services.IPdfExportService, EventSeatingPlanner.Infrastructure.Services.PdfExportService>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Services.IAssetStorage, EventSeatingPlanner.Infrastructure.Storage.InMemoryAssetStorage>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Services.IAssetService, EventSeatingPlanner.Application.Services.AssetService>();
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Services.IAuthService, EventSeatingPlanner.Infrastructure.Services.AuthService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();
