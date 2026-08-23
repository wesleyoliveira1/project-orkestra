using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.UnitTests.Domain;

public class BusinessUnitTests {
    private static readonly Guid ValidOrganizationId = Guid.NewGuid();
    private const string ValidCnpj = "11.222.333/0001-81";
    private const string ValidName = "Loja 1";
    private const string ValidAddress = "Rua das Flores, 123";

    private static BusinessUnit CreateValidBusinessUnit() =>
        new(ValidOrganizationId, ValidName, ValidCnpj, ValidAddress);

    [Fact]
    public void Constructor_WithValidData_CreatesBusinessUnitAsActive() {
        BusinessUnit businessUnit = CreateValidBusinessUnit();

        Assert.Equal(BusinessUnitStatus.Active, businessUnit.Status);
        Assert.NotEqual(Guid.Empty, businessUnit.Id);
        Assert.Null(businessUnit.UpdatedAt);
    }

    [Fact]
    public void Constructor_WithEmptyOrganizationId_ThrowsArgumentException() {
        Assert.Throws<ArgumentException>(() =>
            new BusinessUnit(Guid.Empty, ValidName, ValidCnpj, ValidAddress));
    }

    [Fact]
    public void Constructor_WithEmptyAddress_ThrowsArgumentException() {
        Assert.Throws<ArgumentException>(() =>
            new BusinessUnit(ValidOrganizationId, ValidName, ValidCnpj, ""));
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    public void Constructor_WithInvalidCnpj_ThrowsArgumentException(string invalidCnpj) {
        Assert.Throws<ArgumentException>(() =>
            new BusinessUnit(ValidOrganizationId, ValidName, invalidCnpj, ValidAddress));
    }

    [Fact]
    public void ChangeAddress_WithValidAddress_UpdatesAddressAndTimestamp() {
        BusinessUnit businessUnit = CreateValidBusinessUnit();

        businessUnit.ChangeAddress("Av. Principal, 500");

        Assert.Equal("Av. Principal, 500", businessUnit.Address);
        Assert.NotNull(businessUnit.UpdatedAt);
    }

    [Fact]
    public void ChangeAddress_WithEmptyAddress_ThrowsArgumentException() {
        BusinessUnit businessUnit = CreateValidBusinessUnit();

        Assert.Throws<ArgumentException>(() => businessUnit.ChangeAddress(""));
    }

    [Fact]
    public void Deactivate_SetsStatusToInactiveAndUpdatesTimestamp() {
        BusinessUnit businessUnit = CreateValidBusinessUnit();

        businessUnit.Deactivate();

        Assert.Equal(BusinessUnitStatus.Inactive, businessUnit.Status);
        Assert.NotNull(businessUnit.UpdatedAt);
    }
}