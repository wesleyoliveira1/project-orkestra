using MongoDB.Driver;
using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Infrastructure.Data;

public interface IMongoDbContext
{
    IMongoCollection<Tenant> Tenants { get; }
    IMongoCollection<Organization> Organizations{ get; }
    IMongoCollection<BusinessUnit> BusinessUnits { get; }
    IMongoCollection<Employee> Employees { get; }

}