using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.UnitTests.Domain;

public class TenantTests {
    private const string ValidCnpj = "11.222.333/0001-81";
    private const string ValidName = "Drogaria Araújo";

    private static Tenant CreateValidTenant() =>
        new(ValidName, ValidCnpj);

    [Fact]
    public void Constructor_WithValidData_CreatesTenantAsActive() {
        Tenant tenant = CreateValidTenant();

        Assert.Equal(TenantStatus.Active, tenant.Status);
        Assert.NotEqual(Guid.Empty, tenant.Id);
        Assert.Null(tenant.UpdatedAt);
    }

    [Fact]
    public void Constructor_WithEmptyName_ThrowsArgumentException() {
        Assert.Throws<ArgumentException>(() =>
            new Tenant("", ValidCnpj));
    }

    [Fact]
    public void Constructor_WithSingleCharacterName_ThrowsArgumentException() {
        Assert.Throws<ArgumentException>(() =>
            new Tenant("A", ValidCnpj));
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("00.000.000/0000-00")]
    public void Constructor_WithInvalidCnpj_ThrowsArgumentException(string invalidCnpj) {
        Assert.Throws<ArgumentException>(() =>
            new Tenant(ValidName, invalidCnpj));
    }

    [Fact]
    public void Rename_WithValidName_UpdatesNameAndTimestamp() {
        Tenant tenant = CreateValidTenant();

        tenant.Rename("Drogaria Nova");

        Assert.Equal("Drogaria Nova", tenant.Name);
        Assert.NotNull(tenant.UpdatedAt);
    }

    [Fact]
    public void Rename_WithEmptyName_ThrowsArgumentException() {
        Tenant tenant = CreateValidTenant();

        Assert.Throws<ArgumentException>(() => tenant.Rename(""));
    }

    [Fact]
    public void Deactivate_SetsStatusToInactiveAndUpdatesTimestamp() {
        Tenant tenant = CreateValidTenant();

        tenant.Deactivate();

        Assert.Equal(TenantStatus.Inactive, tenant.Status);
        Assert.NotNull(tenant.UpdatedAt);
    }

    [Fact]
    public void Activate_AfterDeactivate_SetsStatusToActive() {
        Tenant tenant = CreateValidTenant();
        tenant.Deactivate();

        tenant.Activate();

        Assert.Equal(TenantStatus.Active, tenant.Status);
    }
}