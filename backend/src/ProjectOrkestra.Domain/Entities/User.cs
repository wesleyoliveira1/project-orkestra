using ProjectOrkestra.Domain.Enums;
using ProjectOrkestra.Domain.Validators;

namespace ProjectOrkestra.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }
    public Guid? EmployeeId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public UserStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    private User() { }

    public User(Guid? tenantId, Guid? employeeId, string email, string passwordHash, UserRole role)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException($"Email is required.", nameof(email));
        if (!EmailValidator.IsValid(email))
            throw new ArgumentException("Invalid Email.", nameof(email));
        if (string.IsNullOrEmpty(passwordHash))
            throw new ArgumentException($"Password is required.", nameof(passwordHash));
        if (role == UserRole.PlatformAdmin && tenantId != Guid.Empty)
            throw new ArgumentException("Platform Admin users cannot be assigned to a tenant.");
        if (role != UserRole.PlatformAdmin && tenantId == Guid.Empty)
            throw new ArgumentException("Non-Plataform Admin users must be assigned to a tenant.");
        if (role == UserRole.Employee && employeeId == Guid.Empty)
            throw new ArgumentException("An Employee user must have an employeeId assigned.");

        Id = Guid.NewGuid();
        TenantId = tenantId != Guid.Empty ? tenantId : null;
        EmployeeId = employeeId != Guid.Empty ? employeeId : null;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
        Status = UserStatus.Active;
    }

    public void Activate()
    {
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = UserStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail))
            throw new ArgumentNullException("Email is required.", nameof(newEmail));
        if (!EmailValidator.IsValid(newEmail))
            throw new ArgumentException("Invalid Email.", nameof(newEmail));

        Email = newEmail;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePasswordHash(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentNullException(
                "newPasswordHash is required.",
                nameof(newPasswordHash)
            );

        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }
}
