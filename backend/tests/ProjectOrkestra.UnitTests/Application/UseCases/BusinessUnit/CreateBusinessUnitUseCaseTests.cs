using NSubstitute;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.BusinessUnit;

namespace ProjectOrkestra.UnitTests.Application.UseCases.BusinessUnit;

public class CreateBusinessUnitUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_WithValidData_CallsAddAsyncAndReturnsGeneratedId()
    {
        IBusinessUnitRepository repository = Substitute.For<IBusinessUnitRepository>();
        CreateBusinessUnitUseCase useCase = new CreateBusinessUnitUseCase(repository);

        var dto = new CreateBusinessUnitDto
        {
            OrganizationId = Guid.NewGuid(),
            Name = "Loja 1",
            Cnpj = "11.222.333/0001-81",
            Address = "Rua das Flores, 123"
        };

        var id = await useCase.ExecuteAsync(dto);

        Assert.NotEqual(Guid.Empty, id);
        await repository.Received(1).AddAsync(Arg.Any<ProjectOrkestra.Domain.Entities.BusinessUnit>());
    }
}