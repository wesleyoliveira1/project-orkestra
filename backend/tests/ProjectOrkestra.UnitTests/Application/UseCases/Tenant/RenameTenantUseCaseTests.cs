using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Tenant;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Tenant;

public class RenameTenantUseCaseTests {
    private const string ValidCnpj = "11.222.333/0001-81";

    [Fact]
    public async Task ExecuteAsync_WhenTenantExists_RenamesAndPersists() {
        ITenantRepository repository = Substitute.For<ITenantRepository>();
        RenameTenantUseCase useCase = new RenameTenantUseCase(repository);

        ProjectOrkestra.Domain.Entities.Tenant tenant = new ProjectOrkestra.Domain.Entities.Tenant("Drogaria Araújo", ValidCnpj);
        repository.GetByIdAsync(tenant.Id).Returns(tenant);

        await useCase.ExecuteAsync(tenant.Id, "Nova Razão Social");

        Assert.Equal("Nova Razão Social", tenant.Name);
        await repository.Received(1).UpdateAsync(tenant);
    }

    [Fact]
    public async Task ExecuteAsync_WhenTenantDoesNotExist_ThrowsNotFoundException() {
        ITenantRepository repository = Substitute.For<ITenantRepository>();
        RenameTenantUseCase useCase = new RenameTenantUseCase(repository);

        Guid nonExistentId = Guid.NewGuid();
        repository.GetByIdAsync(nonExistentId).Returns((ProjectOrkestra.Domain.Entities.Tenant?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecuteAsync(nonExistentId, "Novo Nome"));
    }
}