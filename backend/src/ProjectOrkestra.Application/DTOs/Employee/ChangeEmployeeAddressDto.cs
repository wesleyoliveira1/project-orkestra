using System;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Application.DTOs;

public class ChangeEmployeeAddressDto
{
    public string NewAddress { get; set; } = string.Empty;
}