using System;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.DTOs;

public class RenameEmployeeDto
{
    public string NewName { get; set; } = string.Empty;
}
