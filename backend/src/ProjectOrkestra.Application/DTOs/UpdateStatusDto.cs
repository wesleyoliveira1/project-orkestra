using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.DTOs;

public class UpdateStatusDto
{
    public OrganizationStatus TargetStatus { get; set; }
}