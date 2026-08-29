using System;
using System.Threading.Tasks;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.Application.UseCases.Tenant;

public class GetTenantUseCase
{
    private readonly ITenantRepository _repository;

    public GetTenantUseCase(ITenantRepository repository)
    {
        _repository = repository;
    }

    public async Task<Domain.Entities.Tenant?> ExecuteAsync(Guid id)
    {
        var tenant = await _repository.GetByIdAsync(id);

        if (tenant is null)
            throw new NotFoundException($"Tenant with id '{id}' was not found.");

        return tenant;
    }
}
