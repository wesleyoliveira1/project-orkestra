using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;
using ProjectOrkestra.Infrastructure.Repositories;

namespace ProjectOrkestra.IntegrationTests;

public class EmployeeRepositoryTests : IClassFixture<MongoDbTestFixture>
{
    private readonly EmployeeRepository _repository;
    private readonly BusinessUnitRepository _businessUnitRepository;
    private readonly MongoDbTestFixture _fixture;
    private const string ValidCpf = "111.444.777-35";
    private const string ValidEmail = "joao@email.com";
    private const string ValidPhone = "(11) 99999-9999";
    private const string ValidAddress = "Rua das Flores, 123";

    public EmployeeRepositoryTests(MongoDbTestFixture fixture)
    {
        _fixture = fixture;
        _repository = new EmployeeRepository(fixture.Context);
        _businessUnitRepository = new BusinessUnitRepository(fixture.Context);
    }

    [Fact]
    public async Task AddAsync_ThenGetByIdAsync_ReturnsPersistedEmployee()
    {
        // Arrange
        var businessUnitId = Guid.NewGuid();
        var employee = new Employee(businessUnitId, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        // Act
        await _repository.AddAsync(employee);
        var result = await _repository.GetByIdAsync(employee.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(employee.Id, result.Id);
        Assert.Equal(employee.Name, result.Name);
        Assert.Equal(employee.Cpf, result.Cpf);
        Assert.Equal(employee.Email, result.Email);
        Assert.Equal(businessUnitId, result.BusinessUnitId);
        Assert.Equal(EmployeeStatus.Active, result.Status);
    }

    [Fact]
    public async Task GetAllByBusinessUnitIdAsync_WithActiveStatusFilter_ReturnsOnlyActiveEmployees()
    {
        // Arrange
        var businessUnitId = Guid.NewGuid();
        var activeEmployee = new Employee(businessUnitId, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);
        var inactiveEmployee = new Employee(businessUnitId, "Maria Santos", "222.555.888-46", "maria@email.com", "(11) 98888-8888", "Avenida Paulista, 1000");
        inactiveEmployee.Deactivate();

        await _repository.AddAsync(activeEmployee);
        await _repository.AddAsync(inactiveEmployee);

        // Act
        var result = await _repository.GetAllByBusinessUnitIdAsync(businessUnitId, new[] { EmployeeStatus.Active });

        // Assert
        Assert.Single(result);
        Assert.Equal(activeEmployee.Id, result.First().Id);
        Assert.Equal(EmployeeStatus.Active, result.First().Status);
    }

    [Fact]
    public async Task GetAllByBusinessUnitIdAsync_WithInactiveStatusFilter_ReturnsOnlyInactiveEmployees()
    {
        // Arrange
        var businessUnitId = Guid.NewGuid();
        var activeEmployee = new Employee(businessUnitId, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);
        var inactiveEmployee = new Employee(businessUnitId, "Maria Santos", "222.555.888-46", "maria@email.com", "(11) 98888-8888", "Avenida Paulista, 1000");
        inactiveEmployee.Deactivate();

        await _repository.AddAsync(activeEmployee);
        await _repository.AddAsync(inactiveEmployee);

        // Act
        var result = await _repository.GetAllByBusinessUnitIdAsync(businessUnitId, new[] { EmployeeStatus.Inactive });

        // Assert
        Assert.Single(result);
        Assert.Equal(inactiveEmployee.Id, result.First().Id);
        Assert.Equal(EmployeeStatus.Inactive, result.First().Status);
    }

    [Fact]
    public async Task GetAllByBusinessUnitIdAsync_WithMultipleStatusFilters_ReturnsBoth()
    {
        // Arrange
        var businessUnitId = Guid.NewGuid();
        var activeEmployee = new Employee(businessUnitId, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);
        var vacationEmployee = new Employee(businessUnitId, "Maria Santos", "222.555.888-46", "maria@email.com", "(11) 98888-8888", "Avenida Paulista, 1000");
        vacationEmployee.Vacation();

        await _repository.AddAsync(activeEmployee);
        await _repository.AddAsync(vacationEmployee);

        // Act
        var result = await _repository.GetAllByBusinessUnitIdAsync(businessUnitId,
            new[] { EmployeeStatus.Active, EmployeeStatus.Vacation });

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllByBusinessUnitIdAsync_WithDifferentBusinessUnit_ReturnsEmpty()
    {
        // Arrange
        var businessUnitId1 = Guid.NewGuid();
        var businessUnitId2 = Guid.NewGuid();
        var employee = new Employee(businessUnitId1, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        await _repository.AddAsync(employee);

        // Act
        var result = await _repository.GetAllByBusinessUnitIdAsync(businessUnitId2, new[] { EmployeeStatus.Active });

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllByOrganizationIdAsync_WithActiveStatusFilter_ReturnsOnlyActiveEmployees()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        var businessUnit = new BusinessUnit(organizationId, "Farmácia Centro", "11.222.333/0001-81", "Rua das Flores, 123");
        var activeEmployee = new Employee(businessUnit.Id, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);
        var inactiveEmployee = new Employee(businessUnit.Id, "Maria Santos", "222.555.888-46", "maria@email.com", "(11) 98888-8888", "Avenida Paulista, 1000");
        inactiveEmployee.Deactivate();

        await _businessUnitRepository.AddAsync(businessUnit);
        await _repository.AddAsync(activeEmployee);
        await _repository.AddAsync(inactiveEmployee);

        // Act
        var result = await _repository.GetAllByOrganizationIdAsync(organizationId, new[] { EmployeeStatus.Active });

        // Assert
        Assert.Single(result);
        Assert.Equal(activeEmployee.Id, result.First().Id);
        Assert.Equal(EmployeeStatus.Active, result.First().Status);
    }

    [Fact]
    public async Task GetAllByOrganizationIdAsync_WithMultipleBusinessUnits_ReturnsAllMatchingEmployees()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        var businessUnit1 = new BusinessUnit(organizationId, "Farmácia Centro", "11.222.333/0001-81", "Rua das Flores, 123");
        var businessUnit2 = new BusinessUnit(organizationId, "Farmácia Sul", "22.333.444/0001-82", "Avenida Paulista, 1000");

        var employee1 = new Employee(businessUnit1.Id, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);
        var employee2 = new Employee(businessUnit2.Id, "Maria Santos", "222.555.888-46", "maria@email.com", "(11) 98888-8888", "Avenida Paulista, 1000");

        await _businessUnitRepository.AddAsync(businessUnit1);
        await _businessUnitRepository.AddAsync(businessUnit2);
        await _repository.AddAsync(employee1);
        await _repository.AddAsync(employee2);

        // Act
        var result = await _repository.GetAllByOrganizationIdAsync(organizationId, new[] { EmployeeStatus.Active });

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllByOrganizationIdAsync_WithDifferentOrganization_ReturnsEmpty()
    {
        // Arrange
        var organizationId1 = Guid.NewGuid();
        var organizationId2 = Guid.NewGuid();
        var businessUnit = new BusinessUnit(organizationId1, "Farmácia Centro", "11.222.333/0001-81", "Rua das Flores, 123");
        var employee = new Employee(businessUnit.Id, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        await _businessUnitRepository.AddAsync(businessUnit);
        await _repository.AddAsync(employee);

        // Act
        var result = await _repository.GetAllByOrganizationIdAsync(organizationId2, new[] { EmployeeStatus.Active });

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdateAsync_ChangesEmail_AndPersistsChanges()
    {
        // Arrange
        var businessUnitId = Guid.NewGuid();
        var employee = new Employee(businessUnitId, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);
        await _repository.AddAsync(employee);

        // Act
        employee.ChangeEmail("newemail@email.com");
        await _repository.UpdateAsync(employee);

        // Retrieve and Assert
        var result = await _repository.GetByIdAsync(employee.Id);
        Assert.NotNull(result);
        Assert.Equal("newemail@email.com", result.Email);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_ChangesPhone_AndPersistsChanges()
    {
        // Arrange
        var businessUnitId = Guid.NewGuid();
        var employee = new Employee(businessUnitId, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);
        await _repository.AddAsync(employee);

        // Act
        employee.ChangePhone("(21) 98888-8888");
        await _repository.UpdateAsync(employee);

        // Retrieve and Assert
        var result = await _repository.GetByIdAsync(employee.Id);
        Assert.NotNull(result);
        Assert.Equal("(21) 98888-8888", result.Phone);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_ChangesAddress_AndPersistsChanges()
    {
        // Arrange
        var businessUnitId = Guid.NewGuid();
        var employee = new Employee(businessUnitId, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);
        await _repository.AddAsync(employee);

        // Act
        employee.ChangeAddress("Avenida Brasil, 500");
        await _repository.UpdateAsync(employee);

        // Retrieve and Assert
        var result = await _repository.GetByIdAsync(employee.Id);
        Assert.NotNull(result);
        Assert.Equal("Avenida Brasil, 500", result.Address);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_MultipleChanges_PersistsAllChanges()
    {
        // Arrange
        var businessUnitId = Guid.NewGuid();
        var employee = new Employee(businessUnitId, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);
        await _repository.AddAsync(employee);
        var originalCreatedAt = employee.CreatedAt;

        // Act - Change multiple fields and status
        employee.ChangeEmail("updated@email.com");
        employee.ChangePhone("(85) 97777-7777");
        employee.Vacation();
        await _repository.UpdateAsync(employee);

        // Retrieve and Assert
        var result = await _repository.GetByIdAsync(employee.Id);
        Assert.NotNull(result);
        Assert.Equal("updated@email.com", result.Email);
        Assert.Equal("(85) 97777-7777", result.Phone);
        Assert.Equal(EmployeeStatus.Vacation, result.Status);
        Assert.Equal(originalCreatedAt.Date, result.CreatedAt.Date);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_StatusTransition_PersistsStatusChange()
    {
        // Arrange
        var businessUnitId = Guid.NewGuid();
        var employee = new Employee(businessUnitId, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);
        await _repository.AddAsync(employee);

        // Act - Active -> Vacation
        employee.Vacation();
        await _repository.UpdateAsync(employee);

        // Assert
        var result = await _repository.GetByIdAsync(employee.Id);
        Assert.NotNull(result);
        Assert.Equal(EmployeeStatus.Vacation, result.Status);

        // Act - Vacation -> License
        employee.License();
        await _repository.UpdateAsync(employee);

        // Assert
        result = await _repository.GetByIdAsync(employee.Id);
        Assert.NotNull(result);
        Assert.Equal(EmployeeStatus.License, result.Status);
    }
}
