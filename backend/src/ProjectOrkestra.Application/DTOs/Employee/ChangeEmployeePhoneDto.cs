using System;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.DTOs;

public class ChangeEmployeePhoneDto
{
    public string NewPhone { get; set; } = string.Empty;
}
