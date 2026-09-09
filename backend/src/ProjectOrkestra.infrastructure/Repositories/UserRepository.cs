using MongoDB.Driver;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;
using ProjectOrkestra.Infrastructure.Data;

namespace ProjectOrkestra.Infrastructure.Repositories;

public class UserRepository : IUserRepository {

    private readonly IMongoDbContext _context;

    public UserRepository(IMongoDbContext context) {
        _context = context;
    }

    public async Task AddAsync(User user) {
        await _context.Users.InsertOneAsync(user);
    }

    public async Task<User?> GetByIdAsync(Guid id) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq(x => x.Id, id);

        return await _context.Users.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByEmailAsync(string email) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq(x => x.Email, email);

        return await _context.Users.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<User>> GetAllByTenantIdAsync(Guid tenantId, IEnumerable<UserStatus> statuses) {
        FilterDefinition<User> filter = Builders<User>.Filter.And(
            Builders<User>.Filter.Eq(x => x.TenantId, tenantId),
            Builders<User>.Filter.In(x => x.Status, statuses)
        );

        return await _context.Users.Find(filter).ToListAsync();
    }

    public async Task UpdateAsync(User user) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);
        await _context.Users.ReplaceOneAsync(filter, user);
    }
}
