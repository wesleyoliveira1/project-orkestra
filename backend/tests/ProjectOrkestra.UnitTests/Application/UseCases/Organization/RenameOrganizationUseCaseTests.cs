using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Organization;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Organization;

public class RenameOrganizationUseCaseTests
{
    private const string ValidCnpj = "11.222.333/0001-81";

    [Fact]
    public async Task ExecuteAsync_WithValidData_RenamesOrganizationAndPersists()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        RenameOrganizationUseCase useCase = new RenameOrganizationUseCase(repository);

        var organization = new ProjectOrkestra.Domain.Entities.Organization(
            Guid.NewGuid(),
            "Farmácia Central",
            ValidCnpj
        );

        repository.GetByIdAsync(organization.Id).Returns(organization);

        // Act
        await useCase.ExecuteAsync(organization.Id, "Farmácia Nova");

        // Assert
        Assert.Equal("Farmácia Nova", organization.Name);
        await repository.Received(1).UpdateAsync(organization);
    }

    [Fact]
    public async Task ExecuteAsync_WhenOrganizationDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        RenameOrganizationUseCase useCase = new RenameOrganizationUseCase(repository);

        var nonExistentId = Guid.NewGuid();
        repository.GetByIdAsync(nonExistentId).Returns((ProjectOrkestra.Domain.Entities.Organization?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            useCase.ExecuteAsync(nonExistentId, "Farmácia Nova"));
    }

    [Fact]
    public async Task ExecuteAsync_WithValidData_CallsUpdateAsyncWithRenamedOrganization()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        RenameOrganizationUseCase useCase = new RenameOrganizationUseCase(repository);

        var organization = new ProjectOrkestra.Domain.Entities.Organization(
            Guid.NewGuid(),
            "Farmácia Central",
            ValidCnpj
        );

        repository.GetByIdAsync(organization.Id).Returns(organization);

        // Act
        await useCase.ExecuteAsync(organization.Id, "Drogaria Araújo");

        // Assert
        await repository.Received(1).UpdateAsync(Arg.Is<ProjectOrkestra.Domain.Entities.Organization>(org =>
            org.Id == organization.Id && org.Name == "Drogaria Araújo"
        ));
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidNewName_ThrowsArgumentException()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        RenameOrganizationUseCase useCase = new RenameOrganizationUseCase(repository);

        var organization = new ProjectOrkestra.Domain.Entities.Organization(
            Guid.NewGuid(),
            "Farmácia Central",
            ValidCnpj
        );

        repository.GetByIdAsync(organization.Id).Returns(organization);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            useCase.ExecuteAsync(organization.Id, "A"));
    }

    [Fact]
    public async Task ExecuteAsync_WithEmptyNewName_ThrowsArgumentException()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        RenameOrganizationUseCase useCase = new RenameOrganizationUseCase(repository);

        var organization = new ProjectOrkestra.Domain.Entities.Organization(
            Guid.NewGuid(),
            "Farmácia Central",
            ValidCnpj
        );

        repository.GetByIdAsync(organization.Id).Returns(organization);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            useCase.ExecuteAsync(organization.Id, ""));
    }

    [Fact]
    public async Task ExecuteAsync_WithDifferentValidNames_UpdatesOrganizationName()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        RenameOrganizationUseCase useCase = new RenameOrganizationUseCase(repository);

        var organization = new ProjectOrkestra.Domain.Entities.Organization(
            Guid.NewGuid(),
            "Farmácia Central",
            ValidCnpj
        );

        repository.GetByIdAsync(organization.Id).Returns(organization);

        // Act
        await useCase.ExecuteAsync(organization.Id, "Farmácia do Brasil");

        // Assert
        Assert.Equal("Farmácia do Brasil", organization.Name);
    }
}
