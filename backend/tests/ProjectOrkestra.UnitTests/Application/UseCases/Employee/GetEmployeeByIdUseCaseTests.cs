using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Employee;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Employee;

public class GetEmployeeByIdUseCaseTests {
    private const string ValidCpf = "111.444.777-35";
    private const string ValidEmail = "joao@email.com";
    private const string ValidPhone = "(11) 99999-9999";
    private const string ValidAddress = "Rua das Flores, 123";

    [Fact]
    public async Task ExecuteAsync_WhenEmployeeExists_ReturnsEmployee() {
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        GetEmployeeByIdUseCase useCase = new GetEmployeeByIdUseCase(repository);

        ProjectOrkestra.Domain.Entities.Employee employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        repository.GetByIdAsync(employee.Id).Returns(employee);

        var result = await useCase.ExecuteAsync(employee.Id);

        Assert.Equal(employee.Id, result.Id);
    }

    [Fact]
    public async Task ExecuteAsync_WhenEmployeeDoesNotExist_ThrowsNotFoundException() {
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        GetEmployeeByIdUseCase useCase = new GetEmployeeByIdUseCase(repository);

        Guid nonExistentId = Guid.NewGuid();
        repository.GetByIdAsync(nonExistentId).Returns((ProjectOrkestra.Domain.Entities.Employee?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecuteAsync(nonExistentId));
    }
}