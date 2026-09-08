using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.DTOs.User;

public class CreateUserDto {
    public Guid? TenantId { get; set; }
    public Guid? EmployeeId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}
