using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectOrkestra.Application.UseCases.Organization;

public class ListOrganizationsByTenantUseCase
{
	private readonly IOrganizationRepository _repository;

	public ListOrganizationsByTenantUseCase(
		IOrganizationRepository repository)
	{
        _repository = repository;
	}

    public async Task<IEnumerable<Domain.Entities.Organization>> ExecuteAsync(
        Guid tenantId)
    {
        return await _repository.GetAllByTenantIdAsync(tenantId);
    }
}
