using System;
using MongoDB.Driver;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Infrastructure.Data;

namespace ProjectOrkestra.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IMongoDbContext _context;

    public EmployeeRepository(IMongoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Employee employee)
    {
        await _context.Employees.InsertOneAsync(employee);
    }

    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        var filter = Builders<Employee>.Filter.Eq(x => x.Id, id);

        return await _context.Employees.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Employee?>> GetAllByBusinessUnitIdAsync(Guid businessUnitId)
    {
        var filter = Builders<Employee>.Filter.Eq(x => x.BusinessUnitId, businessUnitId);

        return await _context.Employees.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetAllByOrganizationIdAsync(Guid organizationId)
    {
        var businessUnitFilter = Builders<BusinessUnit>.Filter.Eq(
            x => x.OrganizationId,
            organizationId
        );

        var businessUnits = await _context.BusinessUnits.Find(businessUnitFilter).ToListAsync();

        var businessUnitIds = businessUnits.Select(x => x.Id).ToList();

        var employeeFilter = Builders<Employee>.Filter.In(x => x.BusinessUnitId, businessUnitIds);

        return await _context.Employees.Find(employeeFilter).ToListAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        var filter = Builders<Employee>.Filter.Eq(x => x.Id, employee.Id);
        await _context.Employees.ReplaceOneAsync(filter, employee);
    }
}
