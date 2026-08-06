using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Infrastructure.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly List<Tenant> _tenants = new();

    public Task AddAsync(Tenant tenant)
    {
        _tenants.Add(tenant);

        return Task.CompletedTask;
    }

    public Task<Tenant?> GetByIdAsync(Guid id)
    {
        var tenant = _tenants
            .FirstOrDefault(x => x.Id == id);

        return Task.FromResult(tenant);
    }
}