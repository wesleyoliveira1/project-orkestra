using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Domain.Entities;

public class Tenant
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Cnpj { get; private set; } = string.Empty;
    public TenantStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    private Tenant()
    {
    }

    public Tenant(string name, string cnpj)
    {
        
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        if(string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("Cnpj is required.", nameof(cnpj));

        Id = Guid.NewGuid();
        Name = name;
        Cnpj = cnpj;
        Status = TenantStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public void Deactivate() {
        Status = TenantStatus.Inactive;
    }

    public void Activate() {
        Status = TenantStatus.Active;
    }

    public void Rename(string newName) {
        if(string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name is required.", nameof(newName));

        Name = newName;
    }
}