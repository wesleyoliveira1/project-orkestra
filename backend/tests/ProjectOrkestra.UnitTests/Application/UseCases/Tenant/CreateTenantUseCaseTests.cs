using NSubstitute;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Tenant;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Tenant;

public class CreateTenantUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_WithValidData_CallsAddAsyncAndReturnsGeneratedId()
    {
        ITenantRepository repository = Substitute.For<ITenantRepository>();
        CreateTenantUseCase useCase = new CreateTenantUseCase(repository);

        var dto = new CreateTenantDto { Name = "Drogaria Araújo", Cnpj = "11.222.333/0001-81" };

        var id = await useCase.ExecuteAsync(dto);

        Assert.NotEqual(Guid.Empty, id);
        await repository.Received(1).AddAsync(Arg.Any<ProjectOrkestra.Domain.Entities.Tenant>());
    }
}