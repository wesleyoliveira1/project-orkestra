using System;
using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Application.Interfaces;

public interface IEmployeeRepository
{
    Task AddAsync(Employee employee);
    Task<Employee?> GetByIdAsync(Guid id);
    Task<IEnumerable<Employee>> GetAllByBusinessUnitIdAsync(Guid id);
    Task<IEnumerable<Employee>> GetAllByOrganizationIdAsync(Guid id);
    Task UpdateAsync(Employee employee);
}