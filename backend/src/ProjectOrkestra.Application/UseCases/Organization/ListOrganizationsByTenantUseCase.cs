using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Exceptions;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.UseCases.Organization;

public class ListOrganizationsByTenantUseCase
{
    private readonly IOrganizationRepository _repository;

    public ListOrganizationsByTenantUseCase(IOrganizationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Domain.Entities.Organization>> ExecuteAsync(Guid tenantId, IEnumerable<OrganizationStatus>? statuses = null)
    {
        var statusFilter = statuses ?? new[] { OrganizationStatus.Active };

        return await _repository.GetAllByTenantIdAsync(tenantId, statusFilter);
    }
}
