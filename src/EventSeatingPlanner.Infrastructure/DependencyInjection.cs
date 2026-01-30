using EventSeatingPlanner.Application.Interfaces.Repositories;
using EventSeatingPlanner.Infrastructure.Persistence;
using EventSeatingPlanner.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventSeatingPlanner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("DefaultConnection connection string is not configured.");
        }

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IEventRepository, PostgresEventRepository>();
        services.AddScoped<ITableRepository, PostgresTableRepository>();
        services.AddScoped<IGuestRepository, PostgresGuestRepository>();
        services.AddScoped<IAssignmentRepository, PostgresAssignmentRepository>();
        services.AddScoped<IPrintSettingsRepository, PostgresPrintSettingsRepository>();
        services.AddScoped<IAssetRepository, PostgresAssetRepository>();
        services.AddScoped<IUserRepository, PostgresUserRepository>();

        return services;
    }
}
