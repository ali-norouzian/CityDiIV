using CityDiIV.Domain.Contracts.Persistence;
using CityDiIV.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CityDiIV.Persistence;

public static class PersistenceConfigs
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        services.AddDbContextPool<AppWriteDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"),
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        });

        services.AddDbContextPool<AppReadDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"),
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}