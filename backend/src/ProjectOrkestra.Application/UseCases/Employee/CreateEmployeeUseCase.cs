using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Application.UseCases.Employee;

public class CreateEmployeeUseCase
{
    private readonly IEmployeeRepository _repository;

    public CreateEmployeeUseCase(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> ExecuteAsync(CreateEmployeeDto dto)
    {
        var employee = new Domain.Entities.Employee(
            dto.BusinessUnitId,
            dto.Name,
            dto.Cpf,
            dto.Email,
            dto.Phone,
            dto.Address
        );

        await _repository.AddAsync(employee);

        return employee.Id;
    }
}
