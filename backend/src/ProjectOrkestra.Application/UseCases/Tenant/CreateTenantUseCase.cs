using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;

namespace ProjectOrkestra.Application.UseCases.Tenant;

public class CreateTenantUseCase
{
    private readonly ITenantRepository _repository;

    public CreateTenantUseCase(
        ITenantRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> ExecuteAsync(
        CreateTenantDto dto)
    {
        var tenant = new Domain.Entities.Tenant(dto.Name);

        await _repository.AddAsync(tenant);

        return tenant.Id;
    }
}