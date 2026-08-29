using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Employee;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Employee;

public class ChangeEmployeePhoneUseCaseTests
{
    private const string ValidCpf = "111.444.777-35";
    private const string ValidEmail = "joao@email.com";
    private const string ValidPhone = "(11) 99999-9999";
    private const string ValidAddress = "Rua das Flores, 123";

    [Fact]
    public async Task ExecuteAsync_WhenEmployeeExists_ChangesPhoneAndPersists()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ChangeEmployeePhoneUseCase useCase = new ChangeEmployeePhoneUseCase(repository);

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        repository.GetByIdAsync(employee.Id).Returns(employee);

        // Act
        await useCase.ExecuteAsync(employee.Id, "(11) 98888-8888");

        // Assert
        Assert.Equal("(11) 98888-8888", employee.Phone);
        await repository.Received(1).UpdateAsync(employee);
    }

    [Fact]
    public async Task ExecuteAsync_WhenEmployeeDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ChangeEmployeePhoneUseCase useCase = new ChangeEmployeePhoneUseCase(repository);

        var nonExistentId = Guid.NewGuid();
        repository.GetByIdAsync(nonExistentId).Returns((ProjectOrkestra.Domain.Entities.Employee?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            useCase.ExecuteAsync(nonExistentId, "(11) 98888-8888"));
    }

    [Fact]
    public async Task ExecuteAsync_WithValidData_CallsUpdateAsyncWithModifiedEmployee()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ChangeEmployeePhoneUseCase useCase = new ChangeEmployeePhoneUseCase(repository);

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        repository.GetByIdAsync(employee.Id).Returns(employee);

        var newPhone = "(21) 97777-7777";

        // Act
        await useCase.ExecuteAsync(employee.Id, newPhone);

        // Assert
        await repository.Received(1).UpdateAsync(Arg.Is<ProjectOrkestra.Domain.Entities.Employee>(emp =>
            emp.Id == employee.Id && emp.Phone == newPhone
        ));
    }

    [Fact]
    public async Task ExecuteAsync_WithDifferentPhones_UpdatesPhoneCorrectly()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ChangeEmployeePhoneUseCase useCase = new ChangeEmployeePhoneUseCase(repository);

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        repository.GetByIdAsync(employee.Id).Returns(employee);

        // Act
        await useCase.ExecuteAsync(employee.Id, "(85) 96666-6666");

        // Assert
        Assert.Equal("(85) 96666-6666", employee.Phone);
    }
}
