using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Application.Interfaces;

public interface IBusinessUnitRepository
{
    Task AddAsync(BusinessUnit businessUnit);
    Task<BusinessUnit?> GetByIdAsync(Guid id);
    Task<IEnumerable<BusinessUnit?>> GetAllByOrganizationIdAsync(Guid organizationId);
    Task UpdateAsync(BusinessUnit businessUnit);
}
