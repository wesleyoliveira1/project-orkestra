using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;
using ProjectOrkestra.Infrastructure.Repositories;

namespace ProjectOrkestra.IntegrationTests;

public class OrganizationRepositoryTests : IClassFixture<MongoDbTestFixture>
{
    private readonly OrganizationRepository _repository;
    private readonly MongoDbTestFixture _fixture;
    private const string ValidCnpj = "11.222.333/0001-81";

    public OrganizationRepositoryTests(MongoDbTestFixture fixture)
    {
        _fixture = fixture;
        _repository = new OrganizationRepository(fixture.Context);
    }

    [Fact]
    public async Task AddAsync_ThenGetByIdAsync_ReturnsPersistedOrganization()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var organization = new Organization(tenantId, "Farmácia Central", ValidCnpj);

        // Act
        await _repository.AddAsync(organization);
        var result = await _repository.GetByIdAsync(organization.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(organization.Id, result.Id);
        Assert.Equal(organization.Name, result.Name);
        Assert.Equal(organization.Cnpj, result.Cnpj);
        Assert.Equal(tenantId, result.TenantId);
        Assert.Equal(OrganizationStatus.Active, result.Status);
    }

    [Fact]
    public async Task GetAllByTenantIdAsync_WithActiveStatusFilter_ReturnsOnlyActiveOrganizations()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var activeOrganization = new Organization(tenantId, "Farmácia Ativa", ValidCnpj);
        var inactiveOrganization = new Organization(tenantId, "Farmácia Inativa", "22.333.444/0001-82");
        inactiveOrganization.Deactivate();

        await _repository.AddAsync(activeOrganization);
        await _repository.AddAsync(inactiveOrganization);

        // Act
        var result = await _repository.GetAllByTenantIdAsync(tenantId, new[] { OrganizationStatus.Active });

        // Assert
        Assert.Single(result);
        Assert.Equal(activeOrganization.Id, result.First().Id);
        Assert.Equal(OrganizationStatus.Active, result.First().Status);
    }

    [Fact]
    public async Task GetAllByTenantIdAsync_WithInactiveStatusFilter_ReturnsOnlyInactiveOrganizations()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var activeOrganization = new Organization(tenantId, "Farmácia Ativa", ValidCnpj);
        var inactiveOrganization = new Organization(tenantId, "Farmácia Inativa", "22.333.444/0001-82");
        inactiveOrganization.Deactivate();

        await _repository.AddAsync(activeOrganization);
        await _repository.AddAsync(inactiveOrganization);

        // Act
        var result = await _repository.GetAllByTenantIdAsync(tenantId, new[] { OrganizationStatus.Inactive });

        // Assert
        Assert.Single(result);
        Assert.Equal(inactiveOrganization.Id, result.First().Id);
        Assert.Equal(OrganizationStatus.Inactive, result.First().Status);
    }

    [Fact]
    public async Task GetAllByTenantIdAsync_WithMultipleStatusFilters_ReturnsBoth()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var activeOrganization = new Organization(tenantId, "Farmácia Ativa", ValidCnpj);
        var inactiveOrganization = new Organization(tenantId, "Farmácia Inativa", "22.333.444/0001-82");
        inactiveOrganization.Deactivate();

        await _repository.AddAsync(activeOrganization);
        await _repository.AddAsync(inactiveOrganization);

        // Act
        var result = await _repository.GetAllByTenantIdAsync(tenantId, 
            new[] { OrganizationStatus.Active, OrganizationStatus.Inactive });

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllByTenantIdAsync_WithDifferentTenant_ReturnsEmpty()
    {
        // Arrange
        var tenantId1 = Guid.NewGuid();
        var tenantId2 = Guid.NewGuid();
        var organization = new Organization(tenantId1, "Farmácia Central", ValidCnpj);

        await _repository.AddAsync(organization);

        // Act
        var result = await _repository.GetAllByTenantIdAsync(tenantId2, new[] { OrganizationStatus.Active });

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdateAsync_RenamesOrganization_AndPersistsChanges()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var organization = new Organization(tenantId, "Farmácia Central", ValidCnpj);
        await _repository.AddAsync(organization);

        // Act
        organization.Rename("Farmácia Nova");
        await _repository.UpdateAsync(organization);

        // Retrieve and Assert
        var result = await _repository.GetByIdAsync(organization.Id);
        Assert.NotNull(result);
        Assert.Equal("Farmácia Nova", result.Name);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_DeactivatesOrganization_AndPersistsChanges()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var organization = new Organization(tenantId, "Farmácia Central", ValidCnpj);
        await _repository.AddAsync(organization);

        // Act
        organization.Deactivate();
        await _repository.UpdateAsync(organization);

        // Retrieve and Assert
        var result = await _repository.GetByIdAsync(organization.Id);
        Assert.NotNull(result);
        Assert.Equal(OrganizationStatus.Inactive, result.Status);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_MultipleChanges_PersistsAllChanges()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var organization = new Organization(tenantId, "Farmácia Central", ValidCnpj);
        await _repository.AddAsync(organization);
        var originalCreatedAt = organization.CreatedAt;

        // Act - Change name and deactivate
        organization.Rename("Farmácia Atualizada");
        organization.Deactivate();
        await _repository.UpdateAsync(organization);

        // Retrieve and Assert
        var result = await _repository.GetByIdAsync(organization.Id);
        Assert.NotNull(result);
        Assert.Equal("Farmácia Atualizada", result.Name);
        Assert.Equal(OrganizationStatus.Inactive, result.Status);
        Assert.Equal(originalCreatedAt.Date, result.CreatedAt.Date);
        Assert.NotNull(result.UpdatedAt);
    }
}
