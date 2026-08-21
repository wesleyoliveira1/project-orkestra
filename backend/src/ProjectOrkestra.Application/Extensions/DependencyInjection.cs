using Microsoft.Extensions.DependencyInjection;
using ProjectOrkestra.Application.UseCases.Tenant;
using ProjectOrkestra.Application.UseCases.Organization;
using ProjectOrkestra.Application.UseCases.BusinessUnit;
using ProjectOrkestra.Application.UseCases.Employee;

namespace ProjectOrkestra.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateTenantUseCase>();
        services.AddScoped<GetTenantUseCase>();
        services.AddScoped<ListTenantsUseCase>();
        services.AddScoped<RenameTenantUseCase>();
        services.AddScoped<UpdateStatusTenantUseCase>();

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

        services.AddScoped<CreateEmployeeUseCase>();
        services.AddScoped<ChangeEmployeeAddressUseCase>();
        services.AddScoped<ChangeEmployeeCpfUseCase>();
        services.AddScoped<ChangeEmployeeEmailUseCase>();
        services.AddScoped<ChangeEmployeePhoneUseCase>();
        services.AddScoped<GetEmployeeByIdUseCase>();
        services.AddScoped<ListEmployeesByBusinessUnitUseCase>();
        services.AddScoped<ListEmployeesByOrganizationUseCase>();
        services.AddScoped<RenameEmployeeUseCase>();
        services.AddScoped<TransferEmployeeToBusinessUnitUseCase>();
        services.AddScoped<UpdateStatusEmployeeUseCase>();

        return services;
    }
}
