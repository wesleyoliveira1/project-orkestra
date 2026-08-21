using System;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.DTOs;

public class ChangeEmployeeEmailDto
{
    public string NewEmail { get; set; } = string.Empty;
}