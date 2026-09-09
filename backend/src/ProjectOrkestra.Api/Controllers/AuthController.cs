using Microsoft.AspNetCore.Mvc;
using ProjectOrkestra.Application.DTOs.Authentication;
using ProjectOrkestra.Application.UseCases.Authentication;

namespace ProjectOrkestra.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly LoginUseCase _loginUseCase;

    public AuthController(LoginUseCase loginUseCase)
    {
        _loginUseCase = loginUseCase;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        LoginResponse response = await _loginUseCase.ExecuteAsync(request);
        return Ok(response);
    }
}
