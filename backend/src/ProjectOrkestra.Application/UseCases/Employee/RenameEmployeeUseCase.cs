using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Exceptions;
using ProjectOrkestra.Domain.Entities;
using System;

namespace ProjectOrkestra.Application.UseCases.Employee;

public class RenameEmployeeUseCase
{

    private readonly IEmployeeRepository _repository;

    public RenameEmployeeUseCase(IEmployeeRepository repository){

        _repository = repository;
    }

    public async Task ExecuteAsync(Guid id, string newName){

        var employee = await _repository.GetByIdAsync(id);

        if(employee is null)
            throw new NotFoundException($"Employee with id {id} was not found.");

        employee.Rename(newName);

        await _repository.UpdateAsync(employee);
    }
}