using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Organization;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Organization;

public class GetOrganizationUseCaseTests {
    private const string ValidCnpj = "11.222.333/0001-81";

    [Fact]
    public async Task ExecuteAsync_WhenOrganizationExists_ReturnsOrganization() {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        GetOrganizationUseCase useCase = new GetOrganizationUseCase(repository);

        ProjectOrkestra.Domain.Entities.Organization organization = new ProjectOrkestra.Domain.Entities.Organization(Guid.NewGuid(), "Farmácia Central", ValidCnpj);

        repository.GetByIdAsync(organization.Id).Returns(organization);

        // Act
        var result = await useCase.ExecuteAsync(organization.Id);

        // Assert
        Assert.Equal(organization.Id, result.Id);
        Assert.Equal(organization.Name, result.Name);
    }

    [Fact]
    public async Task ExecuteAsync_WhenOrganizationDoesNotExist_ThrowsNotFoundException() {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        GetOrganizationUseCase useCase = new GetOrganizationUseCase(repository);

        Guid nonExistentId = Guid.NewGuid();

        repository.GetByIdAsync(nonExistentId).Returns((ProjectOrkestra.Domain.Entities.Organization?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecuteAsync(nonExistentId));
    }
}