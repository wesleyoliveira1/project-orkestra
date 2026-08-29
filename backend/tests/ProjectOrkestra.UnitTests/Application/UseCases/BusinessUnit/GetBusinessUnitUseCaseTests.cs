using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.BusinessUnit;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.UnitTests.Application.UseCases.BusinessUnit;

public class GetBusinessUnitUseCaseTests {
    private const string ValidCnpj = "11.222.333/0001-81";

    [Fact]
    public async Task ExecuteAsync_WhenBusinessUnitExists_ReturnsBusinessUnit() {
        IBusinessUnitRepository repository = Substitute.For<IBusinessUnitRepository>();
        GetBusinessUnitUseCase useCase = new GetBusinessUnitUseCase(repository);

        ProjectOrkestra.Domain.Entities.BusinessUnit businessUnit = new ProjectOrkestra.Domain.Entities.BusinessUnit(
            Guid.NewGuid(), "Loja 1", ValidCnpj, "Rua das Flores, 123");

        repository.GetByIdAsync(businessUnit.Id).Returns(businessUnit);

        ProjectOrkestra.Domain.Entities.BusinessUnit? result = await useCase.ExecuteAsync(businessUnit.Id);

        Assert.Equal(businessUnit.Id, result.Id);
    }

    [Fact]
    public async Task ExecuteAsync_WhenBusinessUnitDoesNotExist_ThrowsNotFoundException() {
        IBusinessUnitRepository repository = Substitute.For<IBusinessUnitRepository>();
        GetBusinessUnitUseCase useCase = new GetBusinessUnitUseCase(repository);

        Guid nonExistentId = Guid.NewGuid();
        repository.GetByIdAsync(nonExistentId).Returns((ProjectOrkestra.Domain.Entities.BusinessUnit?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecuteAsync(nonExistentId));
    }
}