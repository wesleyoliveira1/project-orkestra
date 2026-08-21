using Microsoft.AspNetCore.Mvc;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.UseCases.Organization;

namespace ProjectOrkestra.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrganizationController : ControllerBase
{
    private readonly CreateOrganizationUseCase _createOrganizationUseCase;
    private readonly GetOrganizationUseCase _getOrganizationUseCase;
    private readonly ListOrganizationsByTenantUseCase _listOrganizationsByTenantUseCase;
    private readonly RenameOrganizationUseCase _renameOrganizationUseCase;
    private readonly UpdateStatusOrganizationUseCase _updateStatusOrganizationUseCase;

    public OrganizationController(
        CreateOrganizationUseCase createOrganizationUseCase,
        GetOrganizationUseCase getOrganizationUseCase,
        ListOrganizationsByTenantUseCase listOrganizationsByTenantUseCase,
        RenameOrganizationUseCase renameOrganizationUseCase,
        UpdateStatusOrganizationUseCase updateStatusOrganizationUseCase)
    {
        _createOrganizationUseCase = createOrganizationUseCase;
        _getOrganizationUseCase = getOrganizationUseCase;
        _listOrganizationsByTenantUseCase = listOrganizationsByTenantUseCase;
        _renameOrganizationUseCase = renameOrganizationUseCase;
        _updateStatusOrganizationUseCase = updateStatusOrganizationUseCase;
    }

    /// <summary>Creates a new organization.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrganizationDto dto)
    {
        var id = await _createOrganizationUseCase.ExecuteAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>Gets an organization by its identifier.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var organization = await _getOrganizationUseCase.ExecuteAsync(id);

        return Ok(organization);
    }

    /// <summary>Lists the organizations of a tenant.</summary>
    [HttpGet]
    public async Task<IActionResult> ListByTenant([FromQuery] Guid tenantId)
    {
        var organizations = await _listOrganizationsByTenantUseCase.ExecuteAsync(tenantId);

        return Ok(organizations);
    }

    /// <summary>Changes an organization's name.</summary>
    [HttpPatch("{id:guid}/rename")]
    public async Task<IActionResult> Rename([FromRoute] Guid id, [FromBody] RenameOrganizationDto dto)
    {
        await _renameOrganizationUseCase.ExecuteAsync(id, dto.NewName);

        return NoContent();
    }

    /// <summary>Changes an organization's status.</summary>
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] UpdateOrganizationStatusDto dto)
    {
        await _updateStatusOrganizationUseCase.ExecuteAsync(id, dto.TargetStatus);

        return NoContent();
    }
}