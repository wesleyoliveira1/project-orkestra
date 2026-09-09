using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.Application.UseCases.User;

public class GetUserUseCase
{
    private readonly IUserRepository _repository;

    public GetUserUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Domain.Entities.User> ExecuteAsync(Guid id)
    {
        Domain.Entities.User? user = await _repository.GetByIdAsync(id);

        if (user is null)
            throw new NotFoundException($"User with id {id} was not found");

        return user;
    }
}
