using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Exceptions;
using ProjectOrkestra.Domain.Entities;
using System;

namespace ProjectOrkestra.Application.UseCases.Employee;

public class ChangeEmployeeEmailUseCase
{

    private readonly IEmployeeRepository _repository;

    public ChangeEmployeeEmailUseCase(IEmployeeRepository repository){

        _repository = repository;
    }

    public async Task ExecuteAsync(Guid id, string newEmail){

        var employee = await _repository.GetByIdAsync(id);

        if(employee is null)
            throw new NotFoundException($"Employee with id {id} was not found.");

        employee.ChangeEmail(newEmail);

        await _repository.UpdateAsync(employee);
    }
}