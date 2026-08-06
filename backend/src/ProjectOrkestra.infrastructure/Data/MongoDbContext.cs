using MongoDB.Driver;
using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Infrastructure.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);

        _database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoCollection<Tenant> Tenants => _database.GetCollection<Tenant>("tenants");
}