using System;
using System.Collections.Generic;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.UseCases.Employee;

public class ListEmployeesByBusinessUnitUseCase
{
    private readonly IEmployeeRepository _repository;

    public ListEmployeesByBusinessUnitUseCase(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Domain.Entities.Employee?>> ExecuteAsync(Guid businessUnitId, IEnumerable<EmployeeStatus>? statuses = null)
    {
        var statusFilter = statuses ?? new[] { EmployeeStatus.Active };

        return await _repository.GetAllByBusinessUnitIdAsync(businessUnitId, statusFilter);
    }
}
