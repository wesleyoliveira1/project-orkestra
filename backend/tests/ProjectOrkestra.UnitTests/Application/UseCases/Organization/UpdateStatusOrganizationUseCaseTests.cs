using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Organization;
using ProjectOrkestra.Domain.Enums;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Organization;

public class UpdateStatusOrganizationUseCaseTests
{
    private const string ValidCnpj = "11.222.333/0001-81";

    [Fact]
    public async Task ExecuteAsync_WithInactiveTarget_DeactivatesAndPersists()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        UpdateStatusOrganizationUseCase useCase = new UpdateStatusOrganizationUseCase(repository);

        var organization = new ProjectOrkestra.Domain.Entities.Organization(
            Guid.NewGuid(),
            "Farmácia Central",
            ValidCnpj
        );

        repository.GetByIdAsync(organization.Id).Returns(organization);

        // Act
        await useCase.ExecuteAsync(organization.Id, OrganizationStatus.Inactive);

        // Assert
        Assert.Equal(OrganizationStatus.Inactive, organization.Status);
        await repository.Received(1).UpdateAsync(organization);
    }

    [Fact]
    public async Task ExecuteAsync_WithActiveTarget_ActivatesAndPersists()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        UpdateStatusOrganizationUseCase useCase = new UpdateStatusOrganizationUseCase(repository);

        var organization = new ProjectOrkestra.Domain.Entities.Organization(
            Guid.NewGuid(),
            "Farmácia Central",
            ValidCnpj
        );
        organization.Deactivate();

        repository.GetByIdAsync(organization.Id).Returns(organization);

        // Act
        await useCase.ExecuteAsync(organization.Id, OrganizationStatus.Active);

        // Assert
        Assert.Equal(OrganizationStatus.Active, organization.Status);
        await repository.Received(1).UpdateAsync(organization);
    }

    [Fact]
    public async Task ExecuteAsync_WhenOrganizationDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        UpdateStatusOrganizationUseCase useCase = new UpdateStatusOrganizationUseCase(repository);

        var nonExistentId = Guid.NewGuid();
        repository.GetByIdAsync(nonExistentId).Returns((ProjectOrkestra.Domain.Entities.Organization?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            useCase.ExecuteAsync(nonExistentId, OrganizationStatus.Active));
    }

    [Fact]
    public async Task ExecuteAsync_DeactivatingAlreadyInactive_RemainsInactive()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        UpdateStatusOrganizationUseCase useCase = new UpdateStatusOrganizationUseCase(repository);

        var organization = new ProjectOrkestra.Domain.Entities.Organization(
            Guid.NewGuid(),
            "Farmácia Central",
            ValidCnpj
        );
        organization.Deactivate();

        repository.GetByIdAsync(organization.Id).Returns(organization);

        // Act
        await useCase.ExecuteAsync(organization.Id, OrganizationStatus.Inactive);

        // Assert
        Assert.Equal(OrganizationStatus.Inactive, organization.Status);
    }

    [Fact]
    public async Task ExecuteAsync_ActivatingAlreadyActive_RemainsActive()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        UpdateStatusOrganizationUseCase useCase = new UpdateStatusOrganizationUseCase(repository);

        var organization = new ProjectOrkestra.Domain.Entities.Organization(
            Guid.NewGuid(),
            "Farmácia Central",
            ValidCnpj
        );

        repository.GetByIdAsync(organization.Id).Returns(organization);

        // Act
        await useCase.ExecuteAsync(organization.Id, OrganizationStatus.Active);

        // Assert
        Assert.Equal(OrganizationStatus.Active, organization.Status);
    }

    [Fact]
    public async Task ExecuteAsync_WithActiveTarget_CallsUpdateAsyncWithActivatedOrganization()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        UpdateStatusOrganizationUseCase useCase = new UpdateStatusOrganizationUseCase(repository);

        var organization = new ProjectOrkestra.Domain.Entities.Organization(
            Guid.NewGuid(),
            "Farmácia Central",
            ValidCnpj
        );
        organization.Deactivate();

        repository.GetByIdAsync(organization.Id).Returns(organization);

        // Act
        await useCase.ExecuteAsync(organization.Id, OrganizationStatus.Active);

        // Assert
        await repository.Received(1).UpdateAsync(Arg.Is<ProjectOrkestra.Domain.Entities.Organization>(org =>
            org.Id == organization.Id && org.Status == OrganizationStatus.Active
        ));
    }

    [Fact]
    public async Task ExecuteAsync_WithInactiveTarget_CallsUpdateAsyncWithDeactivatedOrganization()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        UpdateStatusOrganizationUseCase useCase = new UpdateStatusOrganizationUseCase(repository);

        var organization = new ProjectOrkestra.Domain.Entities.Organization(
            Guid.NewGuid(),
            "Farmácia Central",
            ValidCnpj
        );

        repository.GetByIdAsync(organization.Id).Returns(organization);

        // Act
        await useCase.ExecuteAsync(organization.Id, OrganizationStatus.Inactive);

        // Assert
        await repository.Received(1).UpdateAsync(Arg.Is<ProjectOrkestra.Domain.Entities.Organization>(org =>
            org.Id == organization.Id && org.Status == OrganizationStatus.Inactive
        ));
    }
}
