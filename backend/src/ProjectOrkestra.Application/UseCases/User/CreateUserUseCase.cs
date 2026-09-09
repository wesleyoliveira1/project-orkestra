using ProjectOrkestra.Application.DTOs.User;
using ProjectOrkestra.Application.Interfaces;

namespace ProjectOrkestra.Application.UseCases.User;

public class CreateUserUseCase
{
    private readonly IUserRepository _repository;

    public CreateUserUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> ExecuteAsync(CreateUserDto dto)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        Domain.Entities.User user = new Domain.Entities.User(
            dto.TenantId,
            dto.EmployeeId,
            dto.Email,
            passwordHash,
            dto.Role
        );

        await _repository.AddAsync(user);

        return user.Id;
    }
}
