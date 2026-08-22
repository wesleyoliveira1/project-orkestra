using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.Application.UseCases.Tenant;

public class UpdateStatusTenantUseCase
{
    private readonly ITenantRepository _repository;

    public UpdateStatusTenantUseCase(ITenantRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(Guid id, TenantStatus targetStatus)
    {
        var tenant = await _repository.GetByIdAsync(id);

        if (tenant is null)
            throw new NotFoundException($"Tenant with id '{id}' was not found.");

        if (targetStatus == TenantStatus.Active)
            tenant.Activate();
        else
            tenant.Deactivate();

        await _repository.UpdateAsync(tenant);
    }
}
