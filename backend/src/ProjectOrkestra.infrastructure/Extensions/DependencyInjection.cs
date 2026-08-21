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
        MongoSerializationConfig.Configure();
        
        TenantMap.Configure();
        OrganizationMap.Configure();
        BusinessUnitMap.Configure();
        EmployeeMap.Configure();

        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

        services.AddSingleton<IMongoDbContext, MongoDbContext>();
        
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IBusinessUnitRepository, BusinessUnitRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();


        return services;
    }
}
