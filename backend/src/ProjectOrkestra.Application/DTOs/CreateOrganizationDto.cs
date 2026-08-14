using System;

namespace ProjectOrkestra.Application.DTOs;

public class CreateOrganizationDto
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
}