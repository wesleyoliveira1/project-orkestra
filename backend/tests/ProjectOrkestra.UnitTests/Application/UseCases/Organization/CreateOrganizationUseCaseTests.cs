using NSubstitute;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Organization;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Organization;

public class CreateOrganizationUseCaseTests
{
    private const string ValidCnpj = "11.222.333/0001-81";

    [Fact]
    public async Task ExecuteAsync_WithValidData_CreatesOrganizationAndReturnsId()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        CreateOrganizationUseCase useCase = new CreateOrganizationUseCase(repository);

        var tenantId = Guid.NewGuid();
        var dto = new CreateOrganizationDto
        {
            TenantId = tenantId,
            Name = "Farmácia Central",
            Cnpj = ValidCnpj
        };

        // Act
        var id = await useCase.ExecuteAsync(dto);

        // Assert
        Assert.NotEqual(Guid.Empty, id);
        await repository.Received(1).AddAsync(Arg.Any<ProjectOrkestra.Domain.Entities.Organization>());
    }

    [Fact]
    public async Task ExecuteAsync_WithValidData_CallsAddAsyncWithCorrectOrganization()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        CreateOrganizationUseCase useCase = new CreateOrganizationUseCase(repository);

        var tenantId = Guid.NewGuid();
        var dto = new CreateOrganizationDto
        {
            TenantId = tenantId,
            Name = "Farmácia Central",
            Cnpj = ValidCnpj
        };

        // Act
        await useCase.ExecuteAsync(dto);

        // Assert
        await repository.Received(1).AddAsync(Arg.Is<ProjectOrkestra.Domain.Entities.Organization>(org =>
            org.TenantId == tenantId &&
            org.Name == "Farmácia Central" &&
            org.Cnpj == ValidCnpj
        ));
    }

    [Fact]
    public async Task ExecuteAsync_WithDifferentTenants_CreatesOrganizationsForEachTenant()
    {
        // Arrange
        IOrganizationRepository repository = Substitute.For<IOrganizationRepository>();
        CreateOrganizationUseCase useCase = new CreateOrganizationUseCase(repository);

        var tenantId1 = Guid.NewGuid();
        var tenantId2 = Guid.NewGuid();

        var dto1 = new CreateOrganizationDto
        {
            TenantId = tenantId1,
            Name = "Farmácia Central",
            Cnpj = "11.222.333/0001-81"
        };

        var dto2 = new CreateOrganizationDto
        {
            TenantId = tenantId2,
            Name = "Drogaria Araújo",
            Cnpj = "22.333.444/0001-82"
        };

        // Act
        var id1 = await useCase.ExecuteAsync(dto1);
        var id2 = await useCase.ExecuteAsync(dto2);

        // Assert
        Assert.NotEqual(id1, id2);
        Assert.NotEqual(Guid.Empty, id1);
        Assert.NotEqual(Guid.Empty, id2);
    }
}
