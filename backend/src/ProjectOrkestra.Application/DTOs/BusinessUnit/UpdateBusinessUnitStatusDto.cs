using System;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.DTOs;

public class UpdateBusinessUnitStatusDto
{
    public BusinessUnitStatus TargetStatus { get; set; }
}
