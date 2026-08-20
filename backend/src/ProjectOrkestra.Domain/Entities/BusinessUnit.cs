using System;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Domain.Entities;

public class BusinessUnit
{
	public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public BusinessUnitStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public BusinessUnit() { }

    public BusinessUnit(Guid organizationId, string name, string address)
	{
        if(organizationId == Guid.Empty)
            throw new ArgumentNullException("OrganizationId is required.", nameof(organizationId));
        if(string.IsNullOrEmpty(name))
            throw new ArgumentNullException("Name is required.", nameof(name));
        if(string.IsNullOrEmpty(address))
            throw new ArgumentNullException("Address is required.", nameof(address));

        Id = Guid.NewGuid();
        OrganizationId = organizationId;
        Name = name;
        Address = address;
        Status = BusinessUnitStatus.Active;
        CreatedAt = DateTime.Now;
    }

    public void Deactivate() {
        Status = BusinessUnitStatus.Inactive;
    }

    public void Activate() {
        Status = BusinessUnitStatus.Active;
    }

    public void ChangeAddress(string newAddress) {
        if(string.IsNullOrEmpty(newAddress))
            throw new ArgumentNullException("Address is required.", nameof(newAddress));

        Address = newAddress;
    }

    public void Rename(string newName) {
        if(string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name is required.", nameof(newName));

        Name = newName;
    }
}
