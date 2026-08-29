using NSubstitute;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Employee;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Employee;

public class CreateEmployeeUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_WithValidData_CallsAddAsyncAndReturnsGeneratedId()
    {
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        CreateEmployeeUseCase useCase = new CreateEmployeeUseCase(repository);

        var dto = new CreateEmployeeDto
        {
            BusinessUnitId = Guid.NewGuid(),
            Name = "João Silva",
            Cpf = "111.444.777-35",
            Email = "joao@email.com",
            Phone = "(11) 99999-9999",
            Address = "Rua das Flores, 123"
        };

        var id = await useCase.ExecuteAsync(dto);

        Assert.NotEqual(Guid.Empty, id);
        await repository.Received(1).AddAsync(Arg.Any<ProjectOrkestra.Domain.Entities.Employee>());
    }
}