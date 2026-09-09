using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Infrastructure.Authentication;
using ProjectOrkestra.Infrastructure.Data;
using ProjectOrkestra.Infrastructure.Mappings;
using ProjectOrkestra.Infrastructure.Repositories;

namespace ProjectOrkestra.Infrastructure.Extensions;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    ) {
        MongoSerializationConfig.Configure();

        TenantMap.Configure();
        OrganizationMap.Configure();
        BusinessUnitMap.Configure();
        EmployeeMap.Configure();
        UserMap.Configure();

        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        services.AddSingleton<IMongoDbContext, MongoDbContext>();

        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IBusinessUnitRepository, BusinessUnitRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenService, JwtTokenService>();

        return services;
    }
}
