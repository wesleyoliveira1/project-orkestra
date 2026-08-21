using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.DTOs;

public class UpdateTenantStatusDto
{
    public TenantStatus TargetStatus { get; set; }
}