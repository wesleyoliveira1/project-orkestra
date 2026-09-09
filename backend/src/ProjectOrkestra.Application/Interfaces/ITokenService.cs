using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Application.Interfaces;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}
