using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.UnitTests.Domain;

public class EmployeeTests {
    private static readonly Guid ValidBusinessUnitId = Guid.NewGuid();
    private const string ValidCpf = "111.444.777-35"; // use um CPF válido de teste, com dígito verificador correto
    private const string ValidEmail = "joao@email.com";
    private const string ValidPhone = "(11) 99999-9999";
    private const string ValidAddress = "Rua das Flores, 123";

    private static Employee CreateValidEmployee() =>
        new(ValidBusinessUnitId, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

    [Fact]
    public void Constructor_WithValidData_CreatesEmployeeAsActive() {
        Employee employee = CreateValidEmployee();

        Assert.Equal(EmployeeStatus.Active, employee.Status);
        Assert.NotEqual(Guid.Empty, employee.Id);
        Assert.Null(employee.UpdatedAt);
    }

    [Fact]
    public void Constructor_WithEmptyBusinessUnitId_ThrowsArgumentException() {
        Assert.Throws<ArgumentException>(() =>
            new Employee(Guid.Empty, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress));
    }

    [Fact]
    public void Constructor_WithEmptyName_ThrowsArgumentException() {
        Assert.Throws<ArgumentException>(() =>
            new Employee(ValidBusinessUnitId, "", ValidCpf, ValidEmail, ValidPhone, ValidAddress));
    }

    [Theory]
    [InlineData("joao")]
    [InlineData("joao@")]
    [InlineData("@email.com")]
    public void Constructor_WithInvalidEmail_ThrowsArgumentException(string invalidEmail) {
        Assert.Throws<ArgumentException>(() =>
            new Employee(ValidBusinessUnitId, "João Silva", ValidCpf, invalidEmail, ValidPhone, ValidAddress));
    }

    [Theory]
    [InlineData("123")]
    [InlineData("000.000.000-00")]
    public void Constructor_WithInvalidCpf_ThrowsArgumentException(string invalidCpf) {
        Assert.Throws<ArgumentException>(() =>
            new Employee(ValidBusinessUnitId, "João Silva", invalidCpf, ValidEmail, ValidPhone, ValidAddress));
    }

    [Fact]
    public void Deactivate_SetsStatusToInactiveAndUpdatesTimestamp() {
        Employee employee = CreateValidEmployee();

        employee.Deactivate();

        Assert.Equal(EmployeeStatus.Inactive, employee.Status);
        Assert.NotNull(employee.UpdatedAt);
    }

    [Fact]
    public void TransferToBusinessUnit_ChangesBusinessUnitId() {
        Employee employee = CreateValidEmployee();
        Guid newBusinessUnitId = Guid.NewGuid();

        employee.TransferToBusinessUnit(newBusinessUnitId);

        Assert.Equal(newBusinessUnitId, employee.BusinessUnitId);
    }
}