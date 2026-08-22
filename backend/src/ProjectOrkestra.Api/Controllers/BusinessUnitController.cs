using Microsoft.AspNetCore.Mvc;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.UseCases.BusinessUnit;

namespace ProjectOrkestra.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BusinessUnitController : ControllerBase
{
    private readonly CreateBusinessUnitUseCase _createBusinessUnitUseCase;
    private readonly ChangeBusinessUnitAddressUseCase _changeBusinessUnitAddressUseCase;
    private readonly GetBusinessUnitUseCase _getBusinessUnitUseCase;
    private readonly ListBusinessUnitsByOrganizationUseCase _listBusinessUnitsByOrganizationUseCase;
    private readonly RenameBusinessUnitUseCase _renameBusinessUnitUseCase;
    private readonly UpdateStatusBusinessUnitUseCase _updateStatusBusinessUnitUseCase;

    public BusinessUnitController(
        CreateBusinessUnitUseCase createBusinessUnitUseCase,
        ChangeBusinessUnitAddressUseCase changeBusinessUnitAddressUseCase,
        GetBusinessUnitUseCase getBusinessUnitUseCase,
        ListBusinessUnitsByOrganizationUseCase listBusinessUnitsByOrganizationUseCase,
        RenameBusinessUnitUseCase renameBusinessUnitUseCase,
        UpdateStatusBusinessUnitUseCase updateStatusBusinessUnitUseCase
    )
    {
        _createBusinessUnitUseCase = createBusinessUnitUseCase;
        _changeBusinessUnitAddressUseCase = changeBusinessUnitAddressUseCase;
        _getBusinessUnitUseCase = getBusinessUnitUseCase;
        _listBusinessUnitsByOrganizationUseCase = listBusinessUnitsByOrganizationUseCase;
        _renameBusinessUnitUseCase = renameBusinessUnitUseCase;
        _updateStatusBusinessUnitUseCase = updateStatusBusinessUnitUseCase;
    }

    /// <summary>Creates a new business unit.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBusinessUnitDto dto)
    {
        var id = await _createBusinessUnitUseCase.ExecuteAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>Gets a business unit by its identifier.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var businessUnit = await _getBusinessUnitUseCase.ExecuteAsync(id);

        return Ok(businessUnit);
    }

    /// <summary>Lists the business units of an organization.</summary>
    [HttpGet]
    public async Task<IActionResult> ListByOrganization([FromQuery] Guid organizationId)
    {
        var businessUnits = await _listBusinessUnitsByOrganizationUseCase.ExecuteAsync(
            organizationId
        );

        return Ok(businessUnits);
    }

    /// <summary>Changes a business unit's name.</summary>
    [HttpPatch("{id:guid}/rename")]
    public async Task<IActionResult> Rename(
        [FromRoute] Guid id,
        [FromBody] RenameBusinessUnitDto dto
    )
    {
        await _renameBusinessUnitUseCase.ExecuteAsync(id, dto.NewName);

        return NoContent();
    }

    /// <summary>Changes a business unit's status.</summary>
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(
        [FromRoute] Guid id,
        [FromBody] UpdateBusinessUnitStatusDto dto
    )
    {
        await _updateStatusBusinessUnitUseCase.ExecuteAsync(id, dto.TargetStatus);

        return NoContent();
    }

    /// <summary>Changes a business unit's address.</summary>
    [HttpPatch("{id:guid}/address")]
    public async Task<IActionResult> ChangeAddress(
        [FromRoute] Guid id,
        [FromBody] ChangeBusinessUnitAddressDto dto
    )
    {
        await _changeBusinessUnitAddressUseCase.ExecuteAsync(id, dto.NewAddress);

        return NoContent();
    }
}
