using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Infrastructure.Data;

public class MongoDbContext : IMongoDbContext {
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> options) {
        MongoDbSettings settings = options.Value;

        var client = new MongoClient(settings.ConnectionString);

        _database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoCollection<Tenant> Tenants =>
        _database.GetCollection<Tenant>("tenants");
    public IMongoCollection<Organization> Organizations =>
        _database.GetCollection<Organization>("organizations");
    public IMongoCollection<BusinessUnit> BusinessUnits =>
        _database.GetCollection<BusinessUnit>("businessUnits");
    public IMongoCollection<Employee> Employees =>
        _database.GetCollection<Employee>("employees");
    public IMongoCollection<User> Users =>
        _database.GetCollection<User>("users");
}
