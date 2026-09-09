using Microsoft.AspNetCore.Mvc;
using ProjectOrkestra.Application.DTOs.User;
using ProjectOrkestra.Application.UseCases.User;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly CreateUserUseCase _createUserUseCase;
    private readonly GetUserUseCase _getUserUseCase;
    private readonly ListUsersByTenantUseCase _listUsersByTenantUseCase;

    public UserController(
        CreateUserUseCase createUserUseCase,
        GetUserUseCase getUserUseCase,
        ListUsersByTenantUseCase listUsersByTenantUseCase
    )
    {
        _createUserUseCase = createUserUseCase;
        _getUserUseCase = getUserUseCase;
        _listUsersByTenantUseCase = listUsersByTenantUseCase;
    }

    /// <summary>Creates a new user.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        Guid id = await _createUserUseCase.ExecuteAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>Gets an user by its identifier.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        User? user = await _getUserUseCase.ExecuteAsync(id);

        return Ok(user);
    }

    /// <summary>Lists the users of a tenant.</summary>
    [HttpGet("tenant")]
    public async Task<IActionResult> GetAllByTenantIdAsync(
        [FromQuery] Guid tenantId,
        [FromQuery] IEnumerable<UserStatus>? statuses
    )
    {
        IEnumerable<User?> users = await _listUsersByTenantUseCase.ExecuteAsync(tenantId, statuses);

        return Ok(users);
    }
}
