using System;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.Interfaces;

public interface IEmployeeRepository
{
    Task AddAsync(Employee employee);
    Task<Employee?> GetByIdAsync(Guid id);
    Task<IEnumerable<Employee?>> GetAllByBusinessUnitIdAsync(Guid id, IEnumerable<EmployeeStatus> statuses);
    Task<IEnumerable<Employee>> GetAllByOrganizationIdAsync(Guid id, IEnumerable<EmployeeStatus> statuses);
    Task UpdateAsync(Employee employee);
}
