using MongoDB.Driver;
using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Infrastructure.Data;

public interface IMongoDbContext
{
    IMongoCollection<Tenant> Tenants { get; }
}