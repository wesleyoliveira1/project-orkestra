using System;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.Application.UseCases.Employee;

public class GetEmployeeByIdUseCase
{
    private readonly IEmployeeRepository _repository;

    public GetEmployeeByIdUseCase(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Domain.Entities.Employee?> ExecuteAsync(Guid id)
    {
        var employee = await _repository.GetByIdAsync(id);

        if (employee is null)
            throw new NotFoundException($"Employee with id {id} was not found");

        return employee;
    }
}
