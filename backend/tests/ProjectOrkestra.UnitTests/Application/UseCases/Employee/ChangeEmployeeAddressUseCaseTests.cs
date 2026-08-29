using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Employee;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Employee;

public class ChangeEmployeeAddressUseCaseTests
{
    private const string ValidCpf = "111.444.777-35";
    private const string ValidEmail = "joao@email.com";
    private const string ValidPhone = "(11) 99999-9999";
    private const string ValidAddress = "Rua das Flores, 123";

    [Fact]
    public async Task ExecuteAsync_WhenEmployeeExists_ChangesAddressAndPersists()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ChangeEmployeeAddressUseCase useCase = new ChangeEmployeeAddressUseCase(repository);

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        repository.GetByIdAsync(employee.Id).Returns(employee);

        // Act
        await useCase.ExecuteAsync(employee.Id, "Avenida Paulista, 1000");

        // Assert
        Assert.Equal("Avenida Paulista, 1000", employee.Address);
        await repository.Received(1).UpdateAsync(employee);
    }

    [Fact]
    public async Task ExecuteAsync_WhenEmployeeDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ChangeEmployeeAddressUseCase useCase = new ChangeEmployeeAddressUseCase(repository);

        var nonExistentId = Guid.NewGuid();
        repository.GetByIdAsync(nonExistentId).Returns((ProjectOrkestra.Domain.Entities.Employee?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            useCase.ExecuteAsync(nonExistentId, "Avenida Paulista, 1000"));
    }

    [Fact]
    public async Task ExecuteAsync_WithValidData_CallsUpdateAsyncWithModifiedEmployee()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ChangeEmployeeAddressUseCase useCase = new ChangeEmployeeAddressUseCase(repository);

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        repository.GetByIdAsync(employee.Id).Returns(employee);

        // Act
        await useCase.ExecuteAsync(employee.Id, "Rua Tiradentes, 456");

        // Assert
        await repository.Received(1).UpdateAsync(Arg.Is<ProjectOrkestra.Domain.Entities.Employee>(emp =>
            emp.Id == employee.Id && emp.Address == "Rua Tiradentes, 456"
        ));
    }

    [Fact]
    public async Task ExecuteAsync_WithDifferentAddresses_UpdatesAddressCorrectly()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ChangeEmployeeAddressUseCase useCase = new ChangeEmployeeAddressUseCase(repository);

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        repository.GetByIdAsync(employee.Id).Returns(employee);

        // Act
        await useCase.ExecuteAsync(employee.Id, "Praia da Costa, 789");

        // Assert
        Assert.Equal("Praia da Costa, 789", employee.Address);
    }
}
