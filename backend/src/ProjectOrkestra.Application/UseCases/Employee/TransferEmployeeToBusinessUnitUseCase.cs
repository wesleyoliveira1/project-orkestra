using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.Application.UseCases.Employee;

public class TransferEmployeeToBusinessUnitUseCase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IBusinessUnitRepository _businessUnitRepository;

    public TransferEmployeeToBusinessUnitUseCase(
        IEmployeeRepository employeeRepository,
        IBusinessUnitRepository businessUnitRepository
    )
    {
        _employeeRepository = employeeRepository;
        _businessUnitRepository = businessUnitRepository;
    }

    public async Task ExecuteAsync(Guid employeeId, Guid targetBusinessUnitId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);

        if (employee is null)
            throw new NotFoundException($"Employee with id {employeeId} was not found.");

        var currentBusinessUnit = await _businessUnitRepository.GetByIdAsync(
            employee.BusinessUnitId
        );

        if (currentBusinessUnit is null)
            throw new NotFoundException(
                $"Current BusinessUnit with id {employee.BusinessUnitId} was not found."
            );

        var targetBusinessUnit = await _businessUnitRepository.GetByIdAsync(targetBusinessUnitId);

        if (targetBusinessUnit is null)
            throw new NotFoundException(
                $"Target BusinessUnit with id {targetBusinessUnitId} was not found."
            );

        if (currentBusinessUnit.OrganizationId != targetBusinessUnit.OrganizationId)
            throw new BusinessRuleException(
                "Employee can only be transfered to a BusinessUnit with the same Organization"
            );

        employee.TransferToBusinessUnit(targetBusinessUnit.Id);

        await _employeeRepository.UpdateAsync(employee);
    }
}
