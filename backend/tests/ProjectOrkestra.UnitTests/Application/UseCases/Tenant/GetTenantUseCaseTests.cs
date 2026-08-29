using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Tenant;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Tenant;

public class GetTenantUseCaseTests {
    private const string ValidCnpj = "11.222.333/0001-81";

    [Fact]
    public async Task ExecuteAsync_WhenTenantExists_ReturnsTenant() {
        ITenantRepository repository = Substitute.For<ITenantRepository>();
        GetTenantUseCase useCase = new GetTenantUseCase(repository);

        ProjectOrkestra.Domain.Entities.Tenant tenant = new ProjectOrkestra.Domain.Entities.Tenant("Drogaria Araújo", ValidCnpj);
        repository.GetByIdAsync(tenant.Id).Returns(tenant);

        var result = await useCase.ExecuteAsync(tenant.Id);

        Assert.Equal(tenant.Id, result.Id);
    }

    [Fact]
    public async Task ExecuteAsync_WhenTenantDoesNotExist_ThrowsNotFoundException() {
        ITenantRepository repository = Substitute.For<ITenantRepository>();
        GetTenantUseCase useCase = new GetTenantUseCase(repository);

        Guid nonExistentId = Guid.NewGuid();
        repository.GetByIdAsync(nonExistentId).Returns((ProjectOrkestra.Domain.Entities.Tenant?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecuteAsync(nonExistentId));
    }
}