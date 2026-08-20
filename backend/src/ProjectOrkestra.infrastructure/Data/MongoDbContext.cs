using MongoDB.Driver;
using ProjectOrkestra.Domain.Entities;
using Microsoft.Extensions.Options;

namespace ProjectOrkestra.Infrastructure.Data;

public class MongoDbContext : IMongoDbContext
{
    private readonly IMongoDatabase _database;

   public MongoDbContext(IOptions<MongoDbSettings> options)
{
        var settings = options.Value;

        var client = new MongoClient(settings.ConnectionString);

        _database = client.GetDatabase(settings.DatabaseName);
}

    public IMongoCollection<Tenant> Tenants => _database.GetCollection<Tenant>("tenants");
    public IMongoCollection<Organization> Organizations => _database.GetCollection<Organization>("organizations");
    public IMongoCollection<BusinessUnit> BusinessUnits => _database.GetCollection<BusinessUnit>("businessUnits");
}