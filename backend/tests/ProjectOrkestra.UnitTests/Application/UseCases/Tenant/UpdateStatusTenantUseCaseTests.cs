using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Tenant;
using ProjectOrkestra.Domain.Enums;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Tenant;

public class UpdateStatusTenantUseCaseTests {
    private const string ValidCnpj = "11.222.333/0001-81";

    [Fact]
    public async Task ExecuteAsync_WithInactiveTarget_DeactivatesAndPersists() {
        ITenantRepository repository = Substitute.For<ITenantRepository>();
        UpdateStatusTenantUseCase useCase = new UpdateStatusTenantUseCase(repository);

        ProjectOrkestra.Domain.Entities.Tenant tenant = new ProjectOrkestra.Domain.Entities.Tenant("Drogaria Araújo", ValidCnpj);
        repository.GetByIdAsync(tenant.Id).Returns(tenant);

        await useCase.ExecuteAsync(tenant.Id, TenantStatus.Inactive);

        Assert.Equal(TenantStatus.Inactive, tenant.Status);
        await repository.Received(1).UpdateAsync(tenant);
    }

    [Fact]
    public async Task ExecuteAsync_WhenTenantDoesNotExist_ThrowsNotFoundException() {
        ITenantRepository repository = Substitute.For<ITenantRepository>();
        UpdateStatusTenantUseCase useCase = new UpdateStatusTenantUseCase(repository);

        Guid nonExistentId = Guid.NewGuid();
        repository.GetByIdAsync(nonExistentId).Returns((ProjectOrkestra.Domain.Entities.Tenant?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            useCase.ExecuteAsync(nonExistentId, TenantStatus.Active));
    }
}