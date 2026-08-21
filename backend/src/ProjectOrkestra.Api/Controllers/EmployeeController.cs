using Microsoft.AspNetCore.Mvc;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.UseCases.Employee;

namespace ProjectOrkestra.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly CreateEmployeeUseCase _createEmployeeUseCase;
    private readonly ChangeEmployeeAddressUseCase _changeEmployeeAddressUseCase;
    private readonly ChangeEmployeeCpfUseCase _changeEmployeeCpfUseCase;
    private readonly ChangeEmployeeEmailUseCase _changeEmployeeEmailUseCase;
    private readonly ChangeEmployeePhoneUseCase _changeEmployeePhoneUseCase;
    private readonly GetEmployeeByIdUseCase _getEmployeeByIdUseCase;
    private readonly ListEmployeesByBusinessUnitUseCase _listEmployeesByBusinessUnitUseCase;
    private readonly ListEmployeesByOrganizationUseCase _listEmployeesByOrganizationUseCase;
    private readonly RenameEmployeeUseCase _renameEmployeeUseCase;
    private readonly TransferEmployeeToBusinessUnitUseCase _transferEmployeeToBusinessUnitUseCase;
    private readonly UpdateStatusEmployeeUseCase _updateStatusEmployeeUseCase;

    public EmployeeController(
        CreateEmployeeUseCase createEmployeeUseCase,
        ChangeEmployeeAddressUseCase changeEmployeeAddressUseCase,
        ChangeEmployeeCpfUseCase changeEmployeeCpfUseCase,
        ChangeEmployeeEmailUseCase changeEmployeeEmailUseCase,
        ChangeEmployeePhoneUseCase changeEmployeePhoneUseCase,
        GetEmployeeByIdUseCase getEmployeeByIdUseCase,
        ListEmployeesByBusinessUnitUseCase listEmployeesByBusinessUnitUseCase,
        ListEmployeesByOrganizationUseCase listEmployeesByOrganizationUseCase,
        RenameEmployeeUseCase renameEmployeeUseCase,
        TransferEmployeeToBusinessUnitUseCase transferEmployeeToBusinessUnitUseCase,
        UpdateStatusEmployeeUseCase updateStatusEmployeeUseCase)
    {
        _createEmployeeUseCase = createEmployeeUseCase;
        _changeEmployeeAddressUseCase = changeEmployeeAddressUseCase;
        _changeEmployeeCpfUseCase = changeEmployeeCpfUseCase;
        _changeEmployeeEmailUseCase = changeEmployeeEmailUseCase;
        _changeEmployeePhoneUseCase = changeEmployeePhoneUseCase;
        _getEmployeeByIdUseCase = getEmployeeByIdUseCase;
        _listEmployeesByBusinessUnitUseCase = listEmployeesByBusinessUnitUseCase;
        _listEmployeesByOrganizationUseCase = listEmployeesByOrganizationUseCase;
        _renameEmployeeUseCase = renameEmployeeUseCase;
        _transferEmployeeToBusinessUnitUseCase = transferEmployeeToBusinessUnitUseCase;
        _updateStatusEmployeeUseCase = updateStatusEmployeeUseCase;
    }

    /// <summary>Creates a new employee.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        var id = await _createEmployeeUseCase.ExecuteAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>Gets an employee by its identifier.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var employee = await _getEmployeeByIdUseCase.ExecuteAsync(id);

        return Ok(employee);
    }

    /// <summary>Lists the employees of a business unit.</summary>
    [HttpGet("business-unit")]
    public async Task<IActionResult> ListByBusinessUnit([FromQuery] Guid businessUnitId)
    {
        var employees = await _listEmployeesByBusinessUnitUseCase.ExecuteAsync(businessUnitId);

        return Ok(employees);
    }

    /// <summary>Lists the employees of an organization.</summary>
    [HttpGet("organization")]
    public async Task<IActionResult> ListByOrganization([FromQuery] Guid organizationId)
    {
        var employees = await _listEmployeesByOrganizationUseCase.ExecuteAsync(organizationId);

        return Ok(employees);
    }

    /// <summary>Transfers an employee to another business unit.</summary>
    [HttpPut("{id:guid}/business-unit")]
    public async Task<IActionResult> TransferToBusinessUnit([FromRoute] Guid id, [FromQuery] Guid targetBusinessUnitId)
    {
        await _transferEmployeeToBusinessUnitUseCase.ExecuteAsync(id, targetBusinessUnitId);

        return NoContent();
    }

    /// <summary>Changes an employee's name.</summary>
    [HttpPatch("{id:guid}/rename")]
    public async Task<IActionResult> Rename([FromRoute] Guid id, [FromBody] RenameEmployeeDto dto)
    {
        await _renameEmployeeUseCase.ExecuteAsync(id, dto.NewName);

        return NoContent();
    }

    /// <summary>Changes an employee's address.</summary>
    [HttpPatch("{id:guid}/address")]
    public async Task<IActionResult> ChangeAddress([FromRoute] Guid id, [FromBody] ChangeEmployeeAddressDto dto)
    {
        await _changeEmployeeAddressUseCase.ExecuteAsync(id, dto.NewAddress);

        return NoContent();
    }

    /// <summary>Changes an employee's CPF.</summary>
    [HttpPatch("{id:guid}/cpf")]
    public async Task<IActionResult> ChangeCpf([FromRoute] Guid id, [FromBody] ChangeEmployeeCpfDto dto)
    {
        await _changeEmployeeCpfUseCase.ExecuteAsync(id, dto.NewCpf);

        return NoContent();
    }

    /// <summary>Changes an employee's email address.</summary>
    [HttpPatch("{id:guid}/email")]
    public async Task<IActionResult> ChangeEmail([FromRoute] Guid id, [FromBody] ChangeEmployeeEmailDto dto)
    {
        await _changeEmployeeEmailUseCase.ExecuteAsync(id, dto.NewEmail);

        return NoContent();
    }

    /// <summary>Changes an employee's phone number.</summary>
    [HttpPatch("{id:guid}/phone")]
    public async Task<IActionResult> ChangePhone([FromRoute] Guid id, [FromBody] ChangeEmployeePhoneDto dto)
    {
        await _changeEmployeePhoneUseCase.ExecuteAsync(id, dto.NewPhone);

        return NoContent();
    }

    /// <summary>Changes an employee's status.</summary>
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] UpdateEmployeeStatusDto dto)
    {
        await _updateStatusEmployeeUseCase.ExecuteAsync(id, dto.TargetStatus);

        return NoContent();
    }
}