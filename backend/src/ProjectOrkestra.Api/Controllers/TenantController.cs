using Microsoft.AspNetCore.Mvc;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.UseCases.Tenant;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TenantController : ControllerBase
{
    private readonly CreateTenantUseCase _createTenantUseCase;
    private readonly GetTenantUseCase _getTenantUseCase;
    private readonly ListTenantsUseCase _listTenantsUseCase;
    private readonly RenameTenantUseCase _renameTenantUseCase;
    private readonly UpdateStatusTenantUseCase _updateStatusTenantUseCase;

    public TenantController(
        CreateTenantUseCase createTenantUseCase,
        GetTenantUseCase getTenantUseCase,
        ListTenantsUseCase listTenantsUseCase,
        RenameTenantUseCase renameTenantUseCase,
        UpdateStatusTenantUseCase updateStatusTenantUseCase
    )
    {
        _createTenantUseCase = createTenantUseCase;
        _getTenantUseCase = getTenantUseCase;
        _listTenantsUseCase = listTenantsUseCase;
        _renameTenantUseCase = renameTenantUseCase;
        _updateStatusTenantUseCase = updateStatusTenantUseCase;
    }

    /// <summary>Creates a new tenant.</summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateTenantDto dto)
    {
        var id = await _createTenantUseCase.ExecuteAsync(dto);

        return CreatedAtAction(nameof(Create), new { id }, id);
    }

    /// <summary>Gets a tenant by its identifier.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var tenant = await _getTenantUseCase.ExecuteAsync(id);

        return Ok(tenant);
    }

    /// <summary>Lists all tenants.</summary>
    [HttpGet]
    public async Task<IActionResult> ListTenants()
    {
        var tenants = await _listTenantsUseCase.ExecuteAsync();

        return Ok(tenants);
    }

    /// <summary>Changes a tenant's name.</summary>
    [HttpPatch("{id:guid}/rename")]
    public async Task<IActionResult> Rename([FromRoute] Guid id, [FromBody] RenameTenantDto dto)
    {
        await _renameTenantUseCase.ExecuteAsync(id, dto.NewName);

        return NoContent();
    }

    /// <summary>Changes a tenant's status.</summary>
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(
        [FromRoute] Guid id,
        [FromBody] UpdateTenantStatusDto dto
    )
    {
        await _updateStatusTenantUseCase.ExecuteAsync(id, dto.TargetStatus);

        return NoContent();
    }

    /// <summary>Deactivates a tenant.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> UpdateStatus([FromRoute] Guid id)
    {
        await _updateStatusTenantUseCase.ExecuteAsync(id, TenantStatus.Inactive);

        return NoContent();
    }
}
