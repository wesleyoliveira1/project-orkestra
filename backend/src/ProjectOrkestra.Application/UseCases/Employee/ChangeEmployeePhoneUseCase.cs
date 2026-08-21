using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Exceptions;
using ProjectOrkestra.Domain.Entities;
using System;

namespace ProjectOrkestra.Application.UseCases.Employee;

public class ChangeEmployeePhoneUseCase
{

    private readonly IEmployeeRepository _repository;

    public ChangeEmployeePhoneUseCase(IEmployeeRepository repository){

        _repository = repository;
    }

    public async Task ExecuteAsync(Guid id, string newPhone){

        var employee = await _repository.GetByIdAsync(id);

        if(employee is null)
            throw new NotFoundException($"Employee with id {id} was not found.");

        employee.ChangePhone(newPhone);

        await _repository.UpdateAsync(employee);
    }
}