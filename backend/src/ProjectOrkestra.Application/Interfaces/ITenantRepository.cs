using ProjectOrkestra.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectOrkestra.Application.Interfaces;

public interface ITenantRepository
{
    Task AddAsync(Tenant tenant);
    Task<Tenant?> GetByIdAsync(Guid Id);
    Task<IEnumerable<Tenant>> GetAllAsync();
    Task UpdateAsync(Tenant tenant);
}