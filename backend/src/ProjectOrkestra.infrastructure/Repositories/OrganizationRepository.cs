using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;
using ProjectOrkestra.Infrastructure.Data;

namespace ProjectOrkestra.Infrastructure.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly IMongoDbContext _context;

    public OrganizationRepository(IMongoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Organization organization)
    {
        await _context.Organizations.InsertOneAsync(organization);
    }

    public async Task<Organization?> GetByIdAsync(Guid id)
    {
        var filter = Builders<Organization>.Filter.Eq(x => x.Id, id);

        return await _context.Organizations.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Organization>> GetAllByTenantIdAsync(Guid tenantId, IEnumerable<OrganizationStatus> statuses)
    {
        var filter = Builders<Organization>.Filter.And(
            Builders<Organization>.Filter.Eq(x => x.TenantId, tenantId),
            Builders<Organization>.Filter.In(x => x.Status, statuses)
        );

        return await _context.Organizations.Find(filter).ToListAsync();
    }

    public async Task UpdateAsync(Organization organization)
    {
        var filter = Builders<Organization>.Filter.Eq(x => x.Id, organization.Id);
        await _context.Organizations.ReplaceOneAsync(filter, organization);
    }
}
