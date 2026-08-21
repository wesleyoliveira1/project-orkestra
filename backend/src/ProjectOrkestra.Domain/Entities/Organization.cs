using System;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Domain.Entities;

public class Organization
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Cnpj { get; private set; } = string.Empty;
    public OrganizationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    private Organization() { }

    public Organization(Guid tenantId, string name, string cnpj)
	{
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        if(string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("Cnpj is required.", nameof(cnpj));

        Id = Guid.NewGuid();
        TenantId = tenantId;
        Name = name;
        Cnpj = cnpj;
        Status = OrganizationStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public void Deactivate() {
        Status = OrganizationStatus.Inactive;
    }

    public void Activate() {
        Status = OrganizationStatus.Active;
    }

    public void Rename(string newName) {
        if(string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name is required.", nameof(newName));

        Name = newName;
    }
}
