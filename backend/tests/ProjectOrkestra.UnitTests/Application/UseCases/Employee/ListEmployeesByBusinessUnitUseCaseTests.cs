using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Employee;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Employee;

public class ListEmployeesByBusinessUnitUseCaseTests
{
    private const string ValidCpf = "111.444.777-35";
    private const string ValidEmail = "joao@email.com";
    private const string ValidPhone = "(11) 99999-9999";
    private const string ValidAddress = "Rua das Flores, 123";

    [Fact]
    public async Task ExecuteAsync_WhenEmployeesExist_ReturnsListOfEmployees()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ListEmployeesByBusinessUnitUseCase useCase = new ListEmployeesByBusinessUnitUseCase(repository);

        var businessUnitId = Guid.NewGuid();
        var employees = new List<ProjectOrkestra.Domain.Entities.Employee?>
        {
            new(Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress),
            new(Guid.NewGuid(), "Maria Santos", "222.555.888-46", "maria@email.com", "(11) 98888-8888", "Avenida Paulista, 1000")
        };

        repository.GetAllByBusinessUnitIdAsync(businessUnitId, Arg.Any<IEnumerable<EmployeeStatus>>())
            .Returns(employees);

        // Act
        var result = await useCase.ExecuteAsync(businessUnitId);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoEmployeesExist_ReturnsEmptyList()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ListEmployeesByBusinessUnitUseCase useCase = new ListEmployeesByBusinessUnitUseCase(repository);

        var businessUnitId = Guid.NewGuid();
        repository.GetAllByBusinessUnitIdAsync(businessUnitId, Arg.Any<IEnumerable<EmployeeStatus>>())
            .Returns(new List<ProjectOrkestra.Domain.Entities.Employee?>());

        // Act
        var result = await useCase.ExecuteAsync(businessUnitId);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task ExecuteAsync_WithoutStatusFilter_UsesActiveStatusByDefault()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ListEmployeesByBusinessUnitUseCase useCase = new ListEmployeesByBusinessUnitUseCase(repository);

        var businessUnitId = Guid.NewGuid();
        var activeEmployees = new List<ProjectOrkestra.Domain.Entities.Employee?>
        {
            new(Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress)
        };

        repository.GetAllByBusinessUnitIdAsync(businessUnitId, Arg.Any<IEnumerable<EmployeeStatus>>())
            .Returns(activeEmployees);

        // Act
        var result = await useCase.ExecuteAsync(businessUnitId);

        // Assert
        await repository.Received(1).GetAllByBusinessUnitIdAsync(
            businessUnitId,
            Arg.Is<IEnumerable<EmployeeStatus>>(statuses =>
                statuses.Count() == 1 && statuses.Contains(EmployeeStatus.Active)
            )
        );
    }

    [Fact]
    public async Task ExecuteAsync_WithCustomStatusFilter_ReturnsEmployeesWithFilteredStatuses()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ListEmployeesByBusinessUnitUseCase useCase = new ListEmployeesByBusinessUnitUseCase(repository);

        var businessUnitId = Guid.NewGuid();
        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);
        employee.Vacation();

        var vacationEmployees = new List<ProjectOrkestra.Domain.Entities.Employee?> { employee };
        var statusFilter = new[] { EmployeeStatus.Vacation };

        repository.GetAllByBusinessUnitIdAsync(businessUnitId, statusFilter)
            .Returns(vacationEmployees);

        // Act
        var result = await useCase.ExecuteAsync(businessUnitId, statusFilter);

        // Assert
        Assert.Single(result);
        Assert.All(result.OfType<ProjectOrkestra.Domain.Entities.Employee>(), emp =>
            Assert.Equal(EmployeeStatus.Vacation, emp.Status));
    }

    [Fact]
    public async Task ExecuteAsync_WithMultipleStatusFilters_ReturnsEmployeesMatchingAnyStatus()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ListEmployeesByBusinessUnitUseCase useCase = new ListEmployeesByBusinessUnitUseCase(repository);

        var businessUnitId = Guid.NewGuid();
        var activeEmployee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);
        var vacationEmployee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "Maria Santos", "222.555.888-46", "maria@email.com", "(11) 98888-8888", "Avenida Paulista, 1000");
        vacationEmployee.Vacation();

        var employees = new List<ProjectOrkestra.Domain.Entities.Employee?> { activeEmployee, vacationEmployee };
        var statusFilter = new[] { EmployeeStatus.Active, EmployeeStatus.Vacation };

        repository.GetAllByBusinessUnitIdAsync(businessUnitId, statusFilter)
            .Returns(employees);

        // Act
        var result = await useCase.ExecuteAsync(businessUnitId, statusFilter);

        // Assert
        Assert.Equal(2, result.Count());
    }
}
