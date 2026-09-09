using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.Interfaces;

public interface IUserRepository {
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllByTenantIdAsync(Guid tenantId, IEnumerable<UserStatus> statuses);
    Task UpdateAsync(User user);
}
