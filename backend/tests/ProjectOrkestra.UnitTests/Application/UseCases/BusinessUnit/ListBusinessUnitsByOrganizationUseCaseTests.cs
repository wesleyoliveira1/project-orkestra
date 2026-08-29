using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.BusinessUnit;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.UnitTests.Application.UseCases.BusinessUnit;

public class ListBusinessUnitsByOrganizationUseCaseTests {
    private const string ValidCnpj = "11.222.333/0001-81";

    [Fact]
    public async Task ExecuteAsync_WhenBusinessUnitsExist_ReturnsAllOfThem() {
        IBusinessUnitRepository repository = Substitute.For<IBusinessUnitRepository>();
        ListBusinessUnitsByOrganizationUseCase useCase = new ListBusinessUnitsByOrganizationUseCase(repository);

        Guid organizationId = Guid.NewGuid();
        List<ProjectOrkestra.Domain.Entities.BusinessUnit> businessUnits = new List<ProjectOrkestra.Domain.Entities.BusinessUnit>
        {
            new(organizationId, "Loja 1", ValidCnpj, "Rua A, 1"),
            new(organizationId, "Loja 2", ValidCnpj, "Rua B, 2")
        };

        repository.GetAllByOrganizationIdAsync(organizationId, Arg.Any<IEnumerable<BusinessUnitStatus>>())
            .Returns(businessUnits);

        var result = await useCase.ExecuteAsync(organizationId);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoBusinessUnitsExist_ReturnsEmptyList() {
        IBusinessUnitRepository repository = Substitute.For<IBusinessUnitRepository>();
        ListBusinessUnitsByOrganizationUseCase useCase = new ListBusinessUnitsByOrganizationUseCase(repository);

        Guid organizationId = Guid.NewGuid();
        repository.GetAllByOrganizationIdAsync(organizationId, Arg.Any<IEnumerable<BusinessUnitStatus>>())
            .Returns(new List<ProjectOrkestra.Domain.Entities.BusinessUnit>());

        var result = await useCase.ExecuteAsync(organizationId);

        Assert.Empty(result);
    }
}