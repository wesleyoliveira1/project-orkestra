using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Exceptions;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;
using ProjectOrkestra.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ProjectOrkestra.Application.UseCases.Employee;

public class UpdateStatusEmployeeUseCase
{
	private readonly IEmployeeRepository _repository;

	public UpdateStatusEmployeeUseCase(
		IEmployeeRepository repository)
	{
        _repository = repository;
	}

    public async Task ExecuteAsync(Guid id, EmployeeStatus targetStatus){

        var employee = await _repository.GetByIdAsync(id);

        if(employee is null)
            throw new NotFoundException($"Employee with id {id} was not found.");

        if(targetStatus == EmployeeStatus.Active)
            employee.Activate();
        else if(targetStatus == EmployeeStatus.Vacation)
            employee.Vacation();
        else if(targetStatus == EmployeeStatus.FreeDay)
            employee.Freeday();
        else if(targetStatus == EmployeeStatus.License)
            employee.License();
        else
            employee.Deactivate();

        await _repository.UpdateAsync(employee);
    }

}