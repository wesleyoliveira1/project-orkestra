using Microsoft.Extensions.DependencyInjection;
using ProjectOrkestra.Application.UseCases.Tenant;
using ProjectOrkestra.Application.UseCases.Organization;

namespace ProjectOrkestra.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateTenantUseCase>();
        services.AddScoped<CreateOrganizationUseCase>();
        services.AddScoped<GetOrganizationUseCase>();
        services.AddScoped<ListOrganizationsByTenantUseCase>();
        services.AddScoped<RenameOrganizationUseCase>();
        services.AddScoped<UpdateStatusOrganizationUseCase>();
        
        return services;
    }
}
