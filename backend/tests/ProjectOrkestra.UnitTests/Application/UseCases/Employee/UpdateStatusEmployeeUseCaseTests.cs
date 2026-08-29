using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Employee;
using ProjectOrkestra.Domain.Enums;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Employee;

public class UpdateStatusEmployeeUseCaseTests
{
    private const string ValidCpf = "111.444.777-35";
    private const string ValidEmail = "joao@email.com";
    private const string ValidPhone = "(11) 99999-9999";
    private const string ValidAddress = "Rua das Flores, 123";

    [Fact]
    public async Task ExecuteAsync_WithActiveTarget_ActivatesAndPersists()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        UpdateStatusEmployeeUseCase useCase = new UpdateStatusEmployeeUseCase(repository);

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);
        employee.Vacation();

        repository.GetByIdAsync(employee.Id).Returns(employee);

        // Act
        await useCase.ExecuteAsync(employee.Id, EmployeeStatus.Active);

        // Assert
        Assert.Equal(EmployeeStatus.Active, employee.Status);
        await repository.Received(1).UpdateAsync(employee);
    }

    [Fact]
    public async Task ExecuteAsync_WithVacationTarget_SetsVacationStatusAndPersists()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        UpdateStatusEmployeeUseCase useCase = new UpdateStatusEmployeeUseCase(repository);

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        repository.GetByIdAsync(employee.Id).Returns(employee);

        // Act
        await useCase.ExecuteAsync(employee.Id, EmployeeStatus.Vacation);

        // Assert
        Assert.Equal(EmployeeStatus.Vacation, employee.Status);
        await repository.Received(1).UpdateAsync(employee);
    }

    [Fact]
    public async Task ExecuteAsync_WithFreeDayTarget_SetsFreeayStatusAndPersists()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        UpdateStatusEmployeeUseCase useCase = new UpdateStatusEmployeeUseCase(repository);

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        repository.GetByIdAsync(employee.Id).Returns(employee);

        // Act
        await useCase.ExecuteAsync(employee.Id, EmployeeStatus.FreeDay);

        // Assert
        Assert.Equal(EmployeeStatus.FreeDay, employee.Status);
        await repository.Received(1).UpdateAsync(employee);
    }

    [Fact]
    public async Task ExecuteAsync_WithLicenseTarget_SetsLicenseStatusAndPersists()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        UpdateStatusEmployeeUseCase useCase = new UpdateStatusEmployeeUseCase(repository);

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        repository.GetByIdAsync(employee.Id).Returns(employee);

        // Act
        await useCase.ExecuteAsync(employee.Id, EmployeeStatus.License);

        // Assert
        Assert.Equal(EmployeeStatus.License, employee.Status);
        await repository.Received(1).UpdateAsync(employee);
    }

    [Fact]
    public async Task ExecuteAsync_WithInactiveTarget_DeactivatesAndPersists()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        UpdateStatusEmployeeUseCase useCase = new UpdateStatusEmployeeUseCase(repository);

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        repository.GetByIdAsync(employee.Id).Returns(employee);

        // Act
        await useCase.ExecuteAsync(employee.Id, EmployeeStatus.Inactive);

        // Assert
        Assert.Equal(EmployeeStatus.Inactive, employee.Status);
        await repository.Received(1).UpdateAsync(employee);
    }

    [Fact]
    public async Task ExecuteAsync_WhenEmployeeDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        UpdateStatusEmployeeUseCase useCase = new UpdateStatusEmployeeUseCase(repository);

        var nonExistentId = Guid.NewGuid();
        repository.GetByIdAsync(nonExistentId).Returns((ProjectOrkestra.Domain.Entities.Employee?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            useCase.ExecuteAsync(nonExistentId, EmployeeStatus.Active));
    }

    [Fact]
    public async Task ExecuteAsync_WithMultipleStatusTransitions_UpdatesCorrectly()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        UpdateStatusEmployeeUseCase useCase = new UpdateStatusEmployeeUseCase(repository);

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        repository.GetByIdAsync(employee.Id).Returns(employee);

        // Act - Active -> Vacation
        await useCase.ExecuteAsync(employee.Id, EmployeeStatus.Vacation);
        Assert.Equal(EmployeeStatus.Vacation, employee.Status);

        // Act - Vacation -> FreeDay
        await useCase.ExecuteAsync(employee.Id, EmployeeStatus.FreeDay);
        Assert.Equal(EmployeeStatus.FreeDay, employee.Status);

        // Act - FreeDay -> License
        await useCase.ExecuteAsync(employee.Id, EmployeeStatus.License);
        Assert.Equal(EmployeeStatus.License, employee.Status);

        // Act - License -> Active
        await useCase.ExecuteAsync(employee.Id, EmployeeStatus.Active);
        Assert.Equal(EmployeeStatus.Active, employee.Status);

        // Assert
        await repository.Received(4).UpdateAsync(employee);
    }

    [Fact]
    public async Task ExecuteAsync_WithActiveTarget_CallsUpdateAsyncWithActivatedEmployee()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        UpdateStatusEmployeeUseCase useCase = new UpdateStatusEmployeeUseCase(repository);

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);
        employee.Deactivate();

        repository.GetByIdAsync(employee.Id).Returns(employee);

        // Act
        await useCase.ExecuteAsync(employee.Id, EmployeeStatus.Active);

        // Assert
        await repository.Received(1).UpdateAsync(Arg.Is<ProjectOrkestra.Domain.Entities.Employee>(emp =>
            emp.Id == employee.Id && emp.Status == EmployeeStatus.Active
        ));
    }
}
