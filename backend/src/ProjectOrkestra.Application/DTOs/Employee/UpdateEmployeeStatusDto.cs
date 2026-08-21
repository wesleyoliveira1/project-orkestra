using System;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.DTOs;

public class UpdateEmployeeStatusDto
{
    public EmployeeStatus TargetStatus { get; set; }
}