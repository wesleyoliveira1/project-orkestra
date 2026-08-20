using System;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Domain.Entities;

public class BusinessUnit
{
	public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Cnpj { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public BusinessUnitStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    private BusinessUnit() { }

    public BusinessUnit(Guid organizationId, string name, string cnpj, string address)
	{
        if(organizationId == Guid.Empty)
            throw new ArgumentException("OrganizationId is required.", nameof(organizationId));
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        if(string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("Cnpj is required.", nameof(cnpj));
        if(string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address is required.", nameof(address));

        Id = Guid.NewGuid();
        OrganizationId = organizationId;
        Name = name;
        Cnpj = cnpj;
        Address = address;
        Status = BusinessUnitStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public void Deactivate() {
        Status = BusinessUnitStatus.Inactive;
    }

    public void Activate() {
        Status = BusinessUnitStatus.Active;
    }

    public void ChangeAddress(string newAddress) {
        if(string.IsNullOrWhiteSpace(newAddress))
            throw new ArgumentNullException("Address is required.", nameof(newAddress));

        Address = newAddress;
    }

    public void Rename(string newName) {
        if(string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name is required.", nameof(newName));

        Name = newName;
    }
}
