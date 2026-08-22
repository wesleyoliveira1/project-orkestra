using System;
using System.Collections.Generic;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Application.UseCases.Employee;

public class ListEmployeesByOrganizationUseCase
{
    private readonly IEmployeeRepository _repository;

    public ListEmployeesByOrganizationUseCase(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Domain.Entities.Employee?>> ExecuteAsync(Guid organizationId)
    {
        return await _repository.GetAllByOrganizationIdAsync(organizationId);
    }
}
