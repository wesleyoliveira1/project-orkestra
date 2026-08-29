using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Organization;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Organization;

public class ListOrganizationsByTenantUseCaseTests
{
    private const string ValidCnpj = "11.222.333/0001-81";

    [Fact]
    public async Task ExecuteAsync_WhenOrganizationsExist_ReturnsListOfOrganizations()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        ListOrganizationsByTenantUseCase useCase = new ListOrganizationsByTenantUseCase(repository);

        var tenantId = Guid.NewGuid();
        var organizations = new List<ProjectOrkestra.Domain.Entities.Organization>
        {
            new(tenantId, "Farmácia Central", ValidCnpj),
            new(tenantId, "Drogaria Araújo", "22.333.444/0001-82")
        };

        repository.GetAllByTenantIdAsync(tenantId, Arg.Any<IEnumerable<OrganizationStatus>>())
            .Returns(organizations);

        // Act
        var result = await useCase.ExecuteAsync(tenantId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, org => Assert.Equal(tenantId, org.TenantId));
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoOrganizationsExist_ReturnsEmptyList()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        ListOrganizationsByTenantUseCase useCase = new ListOrganizationsByTenantUseCase(repository);

        var tenantId = Guid.NewGuid();
        repository.GetAllByTenantIdAsync(tenantId, Arg.Any<IEnumerable<OrganizationStatus>>())
            .Returns(new List<ProjectOrkestra.Domain.Entities.Organization>());

        // Act
        var result = await useCase.ExecuteAsync(tenantId);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task ExecuteAsync_WithoutStatusFilter_UsesActiveStatusByDefault()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        ListOrganizationsByTenantUseCase useCase = new ListOrganizationsByTenantUseCase(repository);

        var tenantId = Guid.NewGuid();
        var activeOrganizations = new List<ProjectOrkestra.Domain.Entities.Organization>
        {
            new(tenantId, "Farmácia Central", ValidCnpj)
        };

        repository.GetAllByTenantIdAsync(tenantId, Arg.Any<IEnumerable<OrganizationStatus>>())
            .Returns(activeOrganizations);

        // Act
        var result = await useCase.ExecuteAsync(tenantId);

        // Assert
        await repository.Received(1).GetAllByTenantIdAsync(
            tenantId,
            Arg.Is<IEnumerable<OrganizationStatus>>(statuses =>
                statuses.Count() == 1 && statuses.Contains(OrganizationStatus.Active)
            )
        );
    }

    [Fact]
    public async Task ExecuteAsync_WithCustomStatusFilter_ReturnsOrganizationsWithFilteredStatuses()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        ListOrganizationsByTenantUseCase useCase = new ListOrganizationsByTenantUseCase(repository);

        var tenantId = Guid.NewGuid();
        var organization = new ProjectOrkestra.Domain.Entities.Organization(tenantId, "Farmácia Central", ValidCnpj);
        organization.Deactivate();

        var inactiveOrganizations = new List<ProjectOrkestra.Domain.Entities.Organization> { organization };
        var statusFilter = new[] { OrganizationStatus.Inactive };

        repository.GetAllByTenantIdAsync(tenantId, statusFilter)
            .Returns(inactiveOrganizations);

        // Act
        var result = await useCase.ExecuteAsync(tenantId, statusFilter);

        // Assert
        Assert.Single(result);
        Assert.All(result, org => Assert.Equal(OrganizationStatus.Inactive, org.Status));
    }

    [Fact]
    public async Task ExecuteAsync_WithMultipleStatusFilters_ReturnsOrganizationsMatchingAnyStatus()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        ListOrganizationsByTenantUseCase useCase = new ListOrganizationsByTenantUseCase(repository);

        var tenantId = Guid.NewGuid();
        var activeOrganization = new ProjectOrkestra.Domain.Entities.Organization(tenantId, "Farmácia Central", ValidCnpj);
        var inactiveOrganization = new ProjectOrkestra.Domain.Entities.Organization(tenantId, "Drogaria Araújo", "22.333.444/0001-82");
        inactiveOrganization.Deactivate();

        var organizations = new List<ProjectOrkestra.Domain.Entities.Organization>
        {
            activeOrganization,
            inactiveOrganization
        };

        var statusFilter = new[] { OrganizationStatus.Active, OrganizationStatus.Inactive };
        repository.GetAllByTenantIdAsync(tenantId, statusFilter)
            .Returns(organizations);

        // Act
        var result = await useCase.ExecuteAsync(tenantId, statusFilter);

        // Assert
        Assert.Equal(2, result.Count());
    }
}
