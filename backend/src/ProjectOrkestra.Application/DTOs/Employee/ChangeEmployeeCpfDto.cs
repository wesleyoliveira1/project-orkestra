using System;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.DTOs;

public class ChangeEmployeeCpfDto
{
    public string NewCpf { get; set; } = string.Empty;
}