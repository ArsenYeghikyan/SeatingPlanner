using System.Text;
using EventSeatingPlanner.Api.Authentication;
using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Infrastructure;
using EventSeatingPlanner.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Введите JWT токен в формате: Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
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

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// PDF export (implementation in Infrastructure)
builder.Services.AddScoped<EventSeatingPlanner.Application.Interfaces.Services.IPdfExportService, EventSeatingPlanner.Infrastructure.Services.PdfExportService>();

// Storage (for MVP is ok, can be swapped for cloud later)
builder.Services.AddSingleton<EventSeatingPlanner.Application.Interfaces.Services.IAssetStorage, EventSeatingPlanner.Infrastructure.Storage.InMemoryAssetStorage>();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
if (jwtOptions is null || string.IsNullOrWhiteSpace(jwtOptions.Key))
{
    throw new InvalidOperationException("JWT configuration is missing.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrWhiteSpace(accessToken)
                    && (path.StartsWithSegments("/api/assets")
                        || (path.StartsWithSegments("/api/events") && path.Value?.EndsWith("/pdf", StringComparison.OrdinalIgnoreCase) == true)))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Seating Planner API v1");
    });
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Apply migrations
    await db.Database.MigrateAsync();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
