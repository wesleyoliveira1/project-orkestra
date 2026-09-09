using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.UseCases.User;

public class ListUsersByTenantUseCase
{
    private readonly IUserRepository _repository;

    public ListUsersByTenantUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Domain.Entities.User>> ExecuteAsync(
        Guid tenantId,
        IEnumerable<UserStatus>? statuses = null
    )
    {
        IEnumerable<UserStatus> statusFilter = statuses ?? new[] { UserStatus.Active };

        return await _repository.GetAllByTenantIdAsync(tenantId, statusFilter);
    }
}
