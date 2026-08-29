using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.BusinessUnit;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.UnitTests.Application.UseCases.BusinessUnit;

public class RenameBusinessUnitUseCaseTests {
    private const string ValidCnpj = "11.222.333/0001-81";

    [Fact]
    public async Task ExecuteAsync_WhenBusinessUnitExists_RenamesAndPersists() {
        IBusinessUnitRepository repository = Substitute.For<IBusinessUnitRepository>();
        RenameBusinessUnitUseCase useCase = new RenameBusinessUnitUseCase(repository);

        ProjectOrkestra.Domain.Entities.BusinessUnit businessUnit = new ProjectOrkestra.Domain.Entities.BusinessUnit(
            Guid.NewGuid(), "Loja 1", ValidCnpj, "Rua A, 1");

        repository.GetByIdAsync(businessUnit.Id).Returns(businessUnit);

        await useCase.ExecuteAsync(businessUnit.Id, "Loja Renomeada");

        Assert.Equal("Loja Renomeada", businessUnit.Name);
        await repository.Received(1).UpdateAsync(businessUnit);
    }

    [Fact]
    public async Task ExecuteAsync_WhenBusinessUnitDoesNotExist_ThrowsNotFoundException() {
        IBusinessUnitRepository repository = Substitute.For<IBusinessUnitRepository>();
        RenameBusinessUnitUseCase useCase = new RenameBusinessUnitUseCase(repository);

        Guid nonExistentId = Guid.NewGuid();
        repository.GetByIdAsync(nonExistentId).Returns((ProjectOrkestra.Domain.Entities.BusinessUnit?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecuteAsync(nonExistentId, "Novo Nome"));
        await repository.DidNotReceive().UpdateAsync(Arg.Any<ProjectOrkestra.Domain.Entities.BusinessUnit>());
    }
}