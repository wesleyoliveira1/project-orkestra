using System;

namespace ProjectOrkestra.Application.DTOs;

public class CreateBusinessUnitDto
{
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}
