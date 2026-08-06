using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Infrastructure.Data;
using ProjectOrkestra.Infrastructure.Repositories;
using ProjectOrkestra.Infrastructure.Mappings;

namespace ProjectOrkestra.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        TenantMap.Configure();

        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

        services.AddSingleton<IMongoDbContext, MongoDbContext>();
        
        services.AddScoped<ITenantRepository, TenantRepository>();

        return services;
    }
}
