using ProjectOrkestra.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectOrkestra.Application.Interfaces;

public interface IOrganizationRepository
{
    Task AddAsync(Organization organization);
    Task<Organization?> GetByIdAsync(Guid Id);
    Task<IEnumerable<Organization>> GetAllByTenantIdAsync(Guid tenantid);
    Task UpdateAsync(Organization organization);
}
