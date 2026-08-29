using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Infrastructure.Repositories;

namespace ProjectOrkestra.IntegrationTests;

public class TenantRepositoryTests : IClassFixture<MongoDbTestFixture> {
    private readonly TenantRepository _repository;

    public TenantRepositoryTests(MongoDbTestFixture fixture) {
        _repository = new TenantRepository(fixture.Context);
    }

    [Fact]
    public async Task AddAsync_ThenGetByIdAsync_ReturnsPersistedTenant() {
        // Arrange
        Tenant tenant = new Tenant("Drogaria Teste", "11.222.333/0001-81");

        // Act
        await _repository.AddAsync(tenant);
        var result = await _repository.GetByIdAsync(tenant.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tenant.Name, result.Name);
        Assert.Equal(tenant.Cnpj, result.Cnpj);
    }
}