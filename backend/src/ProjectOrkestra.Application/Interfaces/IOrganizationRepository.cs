using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.Interfaces;

public interface IOrganizationRepository
{
    Task AddAsync(Organization organization);
    Task<Organization?> GetByIdAsync(Guid Id);
    Task<IEnumerable<Organization>> GetAllByTenantIdAsync(Guid tenantid, IEnumerable<OrganizationStatus> statuses);
    Task UpdateAsync(Organization organization);
}
