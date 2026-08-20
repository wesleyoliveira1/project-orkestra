using MongoDB.Driver;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectOrkestra.Infrastructure.Repositories;

public class BusinessUnitRepository : IBusinessUnitRepository 
{
    private readonly IMongoDbContext _context;

    public BusinessUnitRepository(
        IMongoDbContext context) {
        _context = context;
    }

    public async Task AddAsync(BusinessUnit businessUnit) 
    {
        await _context.BusinessUnits.InsertOneAsync(businessUnit);
    }

    public async Task<BusinessUnit?> GetByIdAsync(Guid id)
    {
        var filter = Builders<BusinessUnit>.Filter.Eq(x => x.Id, id);

        return await _context.BusinessUnits
            .Find(filter)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<BusinessUnit?>> GetAllByOrganizationIdAsync(Guid organizationId) 
    {
        var filter = Builders<BusinessUnit>.Filter.Eq(x => x.OrganizationId, organizationId);

        return await _context.BusinessUnits
            .Find(filter)
            .ToListAsync();
    }

    public async Task UpdateAsync(BusinessUnit businessUnit)
    {
        var filter = Builders<BusinessUnit>.Filter.Eq(x => x.Id, businessUnit.Id);
        await _context.BusinessUnits.ReplaceOneAsync(filter, businessUnit);
    }
}
