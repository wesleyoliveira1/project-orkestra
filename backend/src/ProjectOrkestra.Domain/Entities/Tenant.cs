using ProjectOrkestra.Domain.Enums;
using ProjectOrkestra.Domain.Validators;

namespace ProjectOrkestra.Domain.Entities;

public class Tenant {
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Cnpj { get; private set; } = string.Empty;
    public TenantStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    private Tenant() {
    }

    public Tenant(string name, string cnpj) {

        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        if(name.Count(c => !char.IsWhiteSpace(c)) < 2)
            throw new ArgumentException("Name must have at least two characters.", nameof(name));
        if(string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("Cnpj is required.", nameof(cnpj));
        if(!BrazilianDocumentValidator.IsValidCnpj(cnpj))
            throw new ArgumentException("Invalid CNPJ.", nameof(cnpj));

        Id = Guid.NewGuid();
        Name = name;
        Cnpj = BrazilianDocumentValidator.FormatCnpj(cnpj);
        Status = TenantStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public void Deactivate() {
        Status = TenantStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate() {
        Status = TenantStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Rename(string newName) {
        if(string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name is required.", nameof(newName));
        if(newName.Count(c => !char.IsWhiteSpace(c)) < 2)
            throw new ArgumentException("Name must have at least two characters.", nameof(newName));

        Name = newName;
        UpdatedAt = DateTime.UtcNow;
    }
}