using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Infrastructure.Data;
using ProjectOrkestra.Infrastructure.Repositories;

namespace ProjectOrkestra.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration
            .GetSection("MongoDbSettings")
            .Get<MongoDbSettings>()!;

        services.AddSingleton(settings);
        services.AddSingleton<MongoDbContext>();
        
        services.AddScoped<ITenantRepository, TenantRepository>();

        return services;
    }
}
