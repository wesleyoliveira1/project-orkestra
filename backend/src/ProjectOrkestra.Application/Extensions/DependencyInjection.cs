using Microsoft.Extensions.DependencyInjection;
using ProjectOrkestra.Application.UseCases.Tenant;

namespace ProjectOrkestra.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateTenantUseCase>();
        
        return services;
    }
}
