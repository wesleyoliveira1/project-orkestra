using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;
using ProjectOrkestra.Infrastructure.Repositories;

namespace ProjectOrkestra.IntegrationTests;

public class BusinessUnitRepositoryTests : IClassFixture<MongoDbTestFixture>
{
    private readonly BusinessUnitRepository _repository;
    private readonly MongoDbTestFixture _fixture;
    private const string ValidCnpj = "11.222.333/0001-81";

    public BusinessUnitRepositoryTests(MongoDbTestFixture fixture)
    {
        _fixture = fixture;
        _repository = new BusinessUnitRepository(fixture.Context);
    }

    [Fact]
    public async Task AddAsync_ThenGetByIdAsync_ReturnsPersistedBusinessUnit()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        var businessUnit = new BusinessUnit(organizationId, "Farmácia Centro", ValidCnpj, "Rua das Flores, 123");

        // Act
        await _repository.AddAsync(businessUnit);
        var result = await _repository.GetByIdAsync(businessUnit.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(businessUnit.Id, result.Id);
        Assert.Equal(businessUnit.Name, result.Name);
        Assert.Equal(businessUnit.Cnpj, result.Cnpj);
        Assert.Equal(organizationId, result.OrganizationId);
        Assert.Equal(BusinessUnitStatus.Active, result.Status);
    }

    [Fact]
    public async Task GetAllByOrganizationIdAsync_WithActiveStatusFilter_ReturnsOnlyActiveBusinessUnits()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        var activeUnit = new BusinessUnit(organizationId, "Farmácia Centro Ativa", ValidCnpj, "Rua das Flores, 123");
        var inactiveUnit = new BusinessUnit(organizationId, "Farmácia Centro Inativa", "22.333.444/0001-82", "Avenida Paulista, 1000");
        inactiveUnit.Deactivate();

        await _repository.AddAsync(activeUnit);
        await _repository.AddAsync(inactiveUnit);

        // Act
        var result = await _repository.GetAllByOrganizationIdAsync(organizationId, new[] { BusinessUnitStatus.Active });

        // Assert
        Assert.Single(result);
        Assert.Equal(activeUnit.Id, result.First().Id);
        Assert.Equal(BusinessUnitStatus.Active, result.First().Status);
    }

    [Fact]
    public async Task GetAllByOrganizationIdAsync_WithInactiveStatusFilter_ReturnsOnlyInactiveBusinessUnits()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        var activeUnit = new BusinessUnit(organizationId, "Farmácia Centro Ativa", ValidCnpj, "Rua das Flores, 123");
        var inactiveUnit = new BusinessUnit(organizationId, "Farmácia Centro Inativa", "22.333.444/0001-82", "Avenida Paulista, 1000");
        inactiveUnit.Deactivate();

        await _repository.AddAsync(activeUnit);
        await _repository.AddAsync(inactiveUnit);

        // Act
        var result = await _repository.GetAllByOrganizationIdAsync(organizationId, new[] { BusinessUnitStatus.Inactive });

        // Assert
        Assert.Single(result);
        Assert.Equal(inactiveUnit.Id, result.First().Id);
        Assert.Equal(BusinessUnitStatus.Inactive, result.First().Status);
    }

    [Fact]
    public async Task GetAllByOrganizationIdAsync_WithMultipleStatusFilters_ReturnsBoth()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        var activeUnit = new BusinessUnit(organizationId, "Farmácia Centro Ativa", ValidCnpj, "Rua das Flores, 123");
        var inactiveUnit = new BusinessUnit(organizationId, "Farmácia Centro Inativa", "22.333.444/0001-82", "Avenida Paulista, 1000");
        inactiveUnit.Deactivate();

        await _repository.AddAsync(activeUnit);
        await _repository.AddAsync(inactiveUnit);

        // Act
        var result = await _repository.GetAllByOrganizationIdAsync(organizationId,
            new[] { BusinessUnitStatus.Active, BusinessUnitStatus.Inactive });

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllByOrganizationIdAsync_WithDifferentOrganization_ReturnsEmpty()
    {
        // Arrange
        var organizationId1 = Guid.NewGuid();
        var organizationId2 = Guid.NewGuid();
        var businessUnit = new BusinessUnit(organizationId1, "Farmácia Centro", ValidCnpj, "Rua das Flores, 123");

        await _repository.AddAsync(businessUnit);

        // Act
        var result = await _repository.GetAllByOrganizationIdAsync(organizationId2, new[] { BusinessUnitStatus.Active });

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdateAsync_RenamesBusinessUnit_AndPersistsChanges()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        var businessUnit = new BusinessUnit(organizationId, "Farmácia Centro", ValidCnpj, "Rua das Flores, 123");
        await _repository.AddAsync(businessUnit);

        // Act
        businessUnit.Rename("Farmácia Centro Nova");
        await _repository.UpdateAsync(businessUnit);

        // Retrieve and Assert
        var result = await _repository.GetByIdAsync(businessUnit.Id);
        Assert.NotNull(result);
        Assert.Equal("Farmácia Centro Nova", result.Name);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_DeactivatesBusinessUnit_AndPersistsChanges()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        var businessUnit = new BusinessUnit(organizationId, "Farmácia Centro", ValidCnpj, "Rua das Flores, 123");
        await _repository.AddAsync(businessUnit);

        // Act
        businessUnit.Deactivate();
        await _repository.UpdateAsync(businessUnit);

        // Retrieve and Assert
        var result = await _repository.GetByIdAsync(businessUnit.Id);
        Assert.NotNull(result);
        Assert.Equal(BusinessUnitStatus.Inactive, result.Status);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_MultipleChanges_PersistsAllChanges()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        var businessUnit = new BusinessUnit(organizationId, "Farmácia Centro", ValidCnpj, "Rua das Flores, 123");
        await _repository.AddAsync(businessUnit);
        var originalCreatedAt = businessUnit.CreatedAt;

        // Act - Change name and deactivate
        businessUnit.Rename("Farmácia Centro Atualizada");
        businessUnit.Deactivate();
        await _repository.UpdateAsync(businessUnit);

        // Retrieve and Assert
        var result = await _repository.GetByIdAsync(businessUnit.Id);
        Assert.NotNull(result);
        Assert.Equal("Farmácia Centro Atualizada", result.Name);
        Assert.Equal(BusinessUnitStatus.Inactive, result.Status);
        Assert.Equal(originalCreatedAt.Date, result.CreatedAt.Date);
        Assert.NotNull(result.UpdatedAt);
    }
}
