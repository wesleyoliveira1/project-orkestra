using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.BusinessUnit;
using ProjectOrkestra.Domain.Enums;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.UnitTests.Application.UseCases.BusinessUnit;

public class UpdateStatusBusinessUnitUseCaseTests {
    private const string ValidCnpj = "11.222.333/0001-81";

    [Fact]
    public async Task ExecuteAsync_WithInactiveTarget_DeactivatesAndPersists() {
        IBusinessUnitRepository repository = Substitute.For<IBusinessUnitRepository>();
        UpdateStatusBusinessUnitUseCase useCase = new UpdateStatusBusinessUnitUseCase(repository);

        ProjectOrkestra.Domain.Entities.BusinessUnit businessUnit = new ProjectOrkestra.Domain.Entities.BusinessUnit(
            Guid.NewGuid(), "Loja 1", ValidCnpj, "Rua A, 1");

        repository.GetByIdAsync(businessUnit.Id).Returns(businessUnit);

        await useCase.ExecuteAsync(businessUnit.Id, BusinessUnitStatus.Inactive);

        Assert.Equal(BusinessUnitStatus.Inactive, businessUnit.Status);
        await repository.Received(1).UpdateAsync(businessUnit);
    }

    [Fact]
    public async Task ExecuteAsync_WhenBusinessUnitDoesNotExist_ThrowsNotFoundException() {
        IBusinessUnitRepository repository = Substitute.For<IBusinessUnitRepository>();
        UpdateStatusBusinessUnitUseCase useCase = new UpdateStatusBusinessUnitUseCase(repository);

        Guid nonExistentId = Guid.NewGuid();
        repository.GetByIdAsync(nonExistentId).Returns((ProjectOrkestra.Domain.Entities.BusinessUnit?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            useCase.ExecuteAsync(nonExistentId, BusinessUnitStatus.Active));
    }
}