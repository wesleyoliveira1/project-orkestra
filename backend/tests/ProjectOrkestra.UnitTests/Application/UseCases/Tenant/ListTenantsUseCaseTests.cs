using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Tenant;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Tenant;

public class ListTenantsUseCaseTests {
    private const string ValidCnpj = "11.222.333/0001-81";

    [Fact]
    public async Task ExecuteAsync_WhenTenantsExist_ReturnsAllOfThem() {
        ITenantRepository repository = Substitute.For<ITenantRepository>();
        ListTenantsUseCase useCase = new ListTenantsUseCase(repository);

        List<ProjectOrkestra.Domain.Entities.Tenant> tenants = new List<ProjectOrkestra.Domain.Entities.Tenant>
        {
            new("Drogaria Araújo", ValidCnpj),
            new("Farmácia Popular", ValidCnpj)
        };

        repository.GetAllAsync().Returns(tenants);

        var result = await useCase.ExecuteAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoTenantsExist_ReturnsEmptyList() {
        ITenantRepository repository = Substitute.For<ITenantRepository>();
        ListTenantsUseCase useCase = new ListTenantsUseCase(repository);

        repository.GetAllAsync().Returns(new List<ProjectOrkestra.Domain.Entities.Tenant>());

        var result = await useCase.ExecuteAsync();

        Assert.Empty(result);
    }
}