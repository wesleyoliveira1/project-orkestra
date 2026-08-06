using Microsoft.Extensions.DependencyInjection;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Infrastructure.Repositories;

namespace ProjectOrkestra.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ITenantRepository, TenantRepository>();

        return services;
    }
}
