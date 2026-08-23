using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.UnitTests.Domain;

public class OrganizationTests {
    private static readonly Guid ValidTenantId = Guid.NewGuid();
    private const string ValidCnpj = "11.222.333/0001-81"; // CNPJ válido de teste, dígitos corretos
    private const string ValidName = "Farmácia Central";

    private static Organization CreateValidOrganization() =>
        new(ValidTenantId, ValidName, ValidCnpj);

    [Fact]
    public void Constructor_WithValidData_CreatesOrganizationAsActive() {
        Organization organization = CreateValidOrganization();

        Assert.Equal(OrganizationStatus.Active, organization.Status);
        Assert.NotEqual(Guid.Empty, organization.Id);
        Assert.Null(organization.UpdatedAt);
    }

    [Fact]
    public void Constructor_WithEmptyName_ThrowsArgumentException() {
        Assert.Throws<ArgumentException>(() =>
            new Organization(ValidTenantId, "", ValidCnpj));
    }

    [Fact]
    public void Constructor_WithSingleCharacterName_ThrowsArgumentException() {
        Assert.Throws<ArgumentException>(() =>
            new Organization(ValidTenantId, "A", ValidCnpj));
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("00.000.000/0000-00")]
    public void Constructor_WithInvalidCnpj_ThrowsArgumentException(string invalidCnpj) {
        Assert.Throws<ArgumentException>(() =>
            new Organization(ValidTenantId, ValidName, invalidCnpj));
    }

    [Fact]
    public void Rename_WithValidName_UpdatesNameAndTimestamp() {
        Organization organization = CreateValidOrganization();

        organization.Rename("Farmácia Nova");

        Assert.Equal("Farmácia Nova", organization.Name);
        Assert.NotNull(organization.UpdatedAt);
    }

    [Fact]
    public void Rename_WithEmptyName_ThrowsArgumentException() {
        Organization organization = CreateValidOrganization();

        Assert.Throws<ArgumentException>(() => organization.Rename(""));
    }

    [Fact]
    public void Deactivate_SetsStatusToInactiveAndUpdatesTimestamp() {
        Organization organization = CreateValidOrganization();

        organization.Deactivate();

        Assert.Equal(OrganizationStatus.Inactive, organization.Status);
        Assert.NotNull(organization.UpdatedAt);
    }

    [Fact]
    public void Activate_AfterDeactivate_SetsStatusToActive() {
        Organization organization = CreateValidOrganization();
        organization.Deactivate();

        organization.Activate();

        Assert.Equal(OrganizationStatus.Active, organization.Status);
    }
}