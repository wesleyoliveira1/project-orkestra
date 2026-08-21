using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Exceptions;
using ProjectOrkestra.Domain.Entities;
using System;

namespace ProjectOrkestra.Application.UseCases.Employee;

public class ChangeEmployeeAddressUseCase
{

    private readonly IEmployeeRepository _repository;

    public ChangeEmployeeAddressUseCase(IEmployeeRepository repository){

        _repository = repository;
    }

    public async Task ExecuteAsync(Guid id, string newAddress){

        var employee = await _repository.GetByIdAsync(id);

        if(employee is null)
            throw new NotFoundException($"Employee unit with id {id} was not found.");

        employee.ChangeAddress(newAddress);

        await _repository.UpdateAsync(employee);
    }
}