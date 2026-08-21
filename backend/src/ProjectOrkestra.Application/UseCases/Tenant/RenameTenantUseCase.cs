using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectOrkestra.Application.UseCases.Tenant;

public class RenameTenantUseCase
{
	private readonly ITenantRepository _repository;

	public RenameTenantUseCase(
		ITenantRepository repository)
	{
        _repository = repository;
	}

    public async Task ExecuteAsync(
        Guid id, string newName)
    {
        var tenant = await _repository.GetByIdAsync(id);

        if(tenant is null)
            throw new NotFoundException($"Tenant with id '{id}' was not found.");

        tenant.Rename(newName);

        await _repository.UpdateAsync(tenant);
    }
}
