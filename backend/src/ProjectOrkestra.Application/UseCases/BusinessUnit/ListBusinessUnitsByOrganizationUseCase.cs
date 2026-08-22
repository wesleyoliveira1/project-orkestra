using System;
using System.Collections.Generic;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Application.UseCases.BusinessUnit;

public class ListBusinessUnitsByOrganizationUseCase
{
    private readonly IBusinessUnitRepository _repository;

    public ListBusinessUnitsByOrganizationUseCase(IBusinessUnitRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Domain.Entities.BusinessUnit?>> ExecuteAsync(Guid organizationId)
    {
        return await _repository.GetAllByOrganizationIdAsync(organizationId);
    }
}
