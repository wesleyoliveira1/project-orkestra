using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ProjectOrkestra.Application.UseCases.Employee;

public class ListEmployeesByBusinessUnitUseCase
{
    private readonly IEmployeeRepository _repository;

    public ListEmployeesByBusinessUnitUseCase(IEmployeeRepository repository){
        _repository = repository;
    }

    public async Task<IEnumerable<Domain.Entities.Employee?>> ExecuteAsync(Guid businessUnitId){

        return await _repository.GetAllByBusinessUnitIdAsync(businessUnitId);
    }
}