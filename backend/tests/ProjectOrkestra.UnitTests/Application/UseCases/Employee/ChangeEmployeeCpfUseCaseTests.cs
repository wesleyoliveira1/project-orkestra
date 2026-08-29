using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Employee;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Employee;

public class ChangeEmployeeCpfUseCaseTests
{
    private const string ValidCpf = "111.444.777-35";
    private const string ValidEmail = "joao@email.com";
    private const string ValidPhone = "(11) 99999-9999";
    private const string ValidAddress = "Rua das Flores, 123";

    [Fact]
    public async Task ExecuteAsync_WhenEmployeeExists_ChangesCpfAndPersists()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ChangeEmployeeCpfUseCase useCase = new ChangeEmployeeCpfUseCase(repository);

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        repository.GetByIdAsync(employee.Id).Returns(employee);

        // Act
        await useCase.ExecuteAsync(employee.Id, "222.555.888-46");

        // Assert
        Assert.Equal("222.555.888-46", employee.Cpf);
        await repository.Received(1).UpdateAsync(employee);
    }

    [Fact]
    public async Task ExecuteAsync_WhenEmployeeDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ChangeEmployeeCpfUseCase useCase = new ChangeEmployeeCpfUseCase(repository);

        var nonExistentId = Guid.NewGuid();
        repository.GetByIdAsync(nonExistentId).Returns((ProjectOrkestra.Domain.Entities.Employee?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            useCase.ExecuteAsync(nonExistentId, "222.555.888-46"));
    }

    [Fact]
    public async Task ExecuteAsync_WithValidData_CallsUpdateAsyncWithModifiedEmployee()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ChangeEmployeeCpfUseCase useCase = new ChangeEmployeeCpfUseCase(repository);

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        repository.GetByIdAsync(employee.Id).Returns(employee);

        var newCpf = "333.666.999-57";

        // Act
        await useCase.ExecuteAsync(employee.Id, newCpf);

        // Assert
        await repository.Received(1).UpdateAsync(Arg.Is<ProjectOrkestra.Domain.Entities.Employee>(emp =>
            emp.Id == employee.Id && emp.Cpf == newCpf
        ));
    }

    [Fact]
    public async Task ExecuteAsync_WithDifferentCpfs_UpdatesCpfCorrectly()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ChangeEmployeeCpfUseCase useCase = new ChangeEmployeeCpfUseCase(repository);

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        repository.GetByIdAsync(employee.Id).Returns(employee);

        // Act
        await useCase.ExecuteAsync(employee.Id, "444.777.111-68");

        // Assert
        Assert.Equal("444.777.111-68", employee.Cpf);
    }
}
