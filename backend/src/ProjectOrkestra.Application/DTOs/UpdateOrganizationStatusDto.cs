using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.DTOs;

public class UpdateOrganizationStatusDto
{
    public OrganizationStatus TargetStatus { get; set; }
}