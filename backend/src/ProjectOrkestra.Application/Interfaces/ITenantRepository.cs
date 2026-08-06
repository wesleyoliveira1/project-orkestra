using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Application.Interfaces;

public interface ITenantRepository
{
    Task AddAsync(Tenant tenant);
    Task<Tenant?> GetByIdAsync(Guid Id);
}