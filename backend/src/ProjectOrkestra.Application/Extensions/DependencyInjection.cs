using Microsoft.Extensions.DependencyInjection;
using ProjectOrkestra.Application.UseCases.Tenant;
using ProjectOrkestra.Application.UseCases.Organization;
using ProjectOrkestra.Application.UseCases.BusinessUnit;

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
        
        services.AddScoped<CreateBusinessUnitUseCase>();
        services.AddScoped<GetBusinessUnitUseCase>();
        services.AddScoped<ListBusinessUnitsByOrganizationUseCase>();
        services.AddScoped<RenameBusinessUnitUseCase>();
        services.AddScoped<ChangeBusinessUnitAddressUseCase>();
        services.AddScoped<UpdateStatusBusinessUnitUseCase>();

        return services;
    }
}
