using Dapper;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using EventSeatingPlanner.Infrastructure.Persistence;
using EventSeatingPlanner.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

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

        RegisterTypeHandlers();

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        var dataSource = dataSourceBuilder.Build();
        services.AddSingleton(dataSource);
        services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();

        services.AddSingleton<IEventRepository, PostgresEventRepository>();
        services.AddSingleton<ITableRepository, PostgresTableRepository>();
        services.AddSingleton<IGuestRepository, PostgresGuestRepository>();
        services.AddSingleton<IAssignmentRepository, PostgresAssignmentRepository>();
        services.AddSingleton<IPrintSettingsRepository, PostgresPrintSettingsRepository>();
        services.AddSingleton<IAssetRepository, PostgresAssetRepository>();

        return services;
    }

    private static void RegisterTypeHandlers()
    {
        if (!SqlMapper.HasTypeHandler(typeof(DateOnly)))
        {
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        }
    }
}
