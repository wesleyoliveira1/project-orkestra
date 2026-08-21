using Microsoft.AspNetCore.Mvc;
using ProjectOrkestra.Domain.Enums;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.UseCases.Tenant;

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
        UpdateStatusTenantUseCase updateStatusTenantUseCase)
    {
        _createTenantUseCase = createTenantUseCase;
        _getTenantUseCase = getTenantUseCase;
        _listTenantsUseCase = listTenantsUseCase;
        _renameTenantUseCase = renameTenantUseCase;
        _updateStatusTenantUseCase = updateStatusTenantUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateTenantDto dto)
    {
        var id = await _createTenantUseCase.ExecuteAsync(dto);

        return CreatedAtAction(
            nameof(Create),
            new { id },
            id);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var tenant = await _getTenantUseCase.ExecuteAsync(id);

        return Ok(tenant);
    }

    [HttpGet]
    public async Task<IActionResult> ListTenants()
    {
        var tenants = await _listTenantsUseCase.ExecuteAsync();

        return Ok(tenants);
    }

    [HttpPatch("{id:guid}/rename")]
    public async Task<IActionResult> Rename([FromRoute] Guid id, [FromBody] RenameTenantDto dto)
    {
        await _renameTenantUseCase.ExecuteAsync(id, dto.NewName);

        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] UpdateTenantStatusDto dto)
    {
        await _updateStatusTenantUseCase.ExecuteAsync(id, dto.TargetStatus);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> UpdateStatus([FromRoute] Guid id)
    {
        await _updateStatusTenantUseCase.ExecuteAsync(id, TenantStatus.Inactive);

        return NoContent();
    }
}