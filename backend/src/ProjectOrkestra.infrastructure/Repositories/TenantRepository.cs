using MongoDB.Driver;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Infrastructure.Data;

namespace ProjectOrkestra.Infrastructure.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly IMongoDbContext _context;

    public TenantRepository(
        IMongoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Tenant tenant)
    {
        await _context.Tenants.InsertOneAsync(tenant);
    }

    public async Task<Tenant?> GetByIdAsync(Guid id)
    {
        var filter = Builders<Tenant>.Filter.Eq(x => x.Id, id);

        return await _context.Tenants
            .Find(filter)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Tenant>> GetAllAsync()
    {
        return await _context.Tenants
            .Find(_ => true)
            .ToListAsync();
    }

    public async Task UpdateAsync(Tenant tenant) {
        var filter = Builders<Tenant>.Filter.Eq(x => x.Id, tenant.Id);
        await _context.Tenants.ReplaceOneAsync(filter, tenant);
    }
}