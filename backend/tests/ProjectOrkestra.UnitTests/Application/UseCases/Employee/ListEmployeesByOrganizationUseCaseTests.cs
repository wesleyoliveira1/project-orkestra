using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Employee;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Employee;

public class ListEmployeesByOrganizationUseCaseTests
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
        ListEmployeesByOrganizationUseCase useCase = new ListEmployeesByOrganizationUseCase(repository);

        var organizationId = Guid.NewGuid();
        var employees = new List<ProjectOrkestra.Domain.Entities.Employee?>
        {
            new(Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress),
            new(Guid.NewGuid(), "Maria Santos", "222.555.888-46", "maria@email.com", "(11) 98888-8888", "Avenida Paulista, 1000")
        };

        repository.GetAllByOrganizationIdAsync(organizationId, Arg.Any<IEnumerable<EmployeeStatus>>())
            .Returns(employees);

        // Act
        var result = await useCase.ExecuteAsync(organizationId);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoEmployeesExist_ReturnsEmptyList()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ListEmployeesByOrganizationUseCase useCase = new ListEmployeesByOrganizationUseCase(repository);

        var organizationId = Guid.NewGuid();
        repository.GetAllByOrganizationIdAsync(organizationId, Arg.Any<IEnumerable<EmployeeStatus>>())
            .Returns(new List<ProjectOrkestra.Domain.Entities.Employee?>());

        // Act
        var result = await useCase.ExecuteAsync(organizationId);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task ExecuteAsync_WithoutStatusFilter_UsesActiveStatusByDefault()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ListEmployeesByOrganizationUseCase useCase = new ListEmployeesByOrganizationUseCase(repository);

        var organizationId = Guid.NewGuid();
        var activeEmployees = new List<ProjectOrkestra.Domain.Entities.Employee?>
        {
            new(Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress)
        };

        repository.GetAllByOrganizationIdAsync(organizationId, Arg.Any<IEnumerable<EmployeeStatus>>())
            .Returns(activeEmployees);

        // Act
        var result = await useCase.ExecuteAsync(organizationId);

        // Assert
        await repository.Received(1).GetAllByOrganizationIdAsync(
            organizationId,
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
        ListEmployeesByOrganizationUseCase useCase = new ListEmployeesByOrganizationUseCase(repository);

        var organizationId = Guid.NewGuid();
        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);
        employee.Freeday();

        var freeDayEmployees = new List<ProjectOrkestra.Domain.Entities.Employee?> { employee };
        var statusFilter = new[] { EmployeeStatus.FreeDay };

        repository.GetAllByOrganizationIdAsync(organizationId, statusFilter)
            .Returns(freeDayEmployees);

        // Act
        var result = await useCase.ExecuteAsync(organizationId, statusFilter);

        // Assert
        Assert.Single(result);
        Assert.All(result.OfType<ProjectOrkestra.Domain.Entities.Employee>(), emp =>
            Assert.Equal(EmployeeStatus.FreeDay, emp.Status));
    }

    [Fact]
    public async Task ExecuteAsync_WithMultipleStatusFilters_ReturnsEmployeesMatchingAnyStatus()
    {
        // Arrange
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ListEmployeesByOrganizationUseCase useCase = new ListEmployeesByOrganizationUseCase(repository);

        var organizationId = Guid.NewGuid();
        var activeEmployee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);
        var licenseEmployee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "Maria Santos", "222.555.888-46", "maria@email.com", "(11) 98888-8888", "Avenida Paulista, 1000");
        licenseEmployee.License();

        var employees = new List<ProjectOrkestra.Domain.Entities.Employee?> { activeEmployee, licenseEmployee };
        var statusFilter = new[] { EmployeeStatus.Active, EmployeeStatus.License };

        repository.GetAllByOrganizationIdAsync(organizationId, statusFilter)
            .Returns(employees);

        // Act
        var result = await useCase.ExecuteAsync(organizationId, statusFilter);

        // Assert
        Assert.Equal(2, result.Count());
    }
}
