using Microsoft.AspNetCore.Mvc;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.UseCases.Tenant;

namespace ProjectOrkestra.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantController : ControllerBase
{
    private readonly CreateTenantUseCase _createTenantUseCase;

    public TenantController(
        CreateTenantUseCase createTenantUseCase)
    {
        _createTenantUseCase = createTenantUseCase;
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
}