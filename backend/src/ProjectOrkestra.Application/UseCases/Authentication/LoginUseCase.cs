using ProjectOrkestra.Application.DTOs.Authentication;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.Application.UseCases.Authentication;

public class LoginUseCase {
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public LoginUseCase(IUserRepository userRepository, ITokenService tokenService) {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> ExecuteAsync(LoginRequest request) {

        Domain.Entities.User? user = await _userRepository.GetByEmailAsync(request.Email);

        if(user is null)
            throw new UnauthorizedCredentialsException("Invalid Credentials");
        if(user.Status == Domain.Enums.UserStatus.Inactive)
            throw new UnauthorizedCredentialsException("User is inactive");

        bool passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if(!passwordValid)
            throw new UnauthorizedCredentialsException("Invalid Credentials");

        (string? token, DateTime expiresAt) = _tokenService.GenerateToken(user);

        return new LoginResponse {
            Token = token,
            ExpiresAt = expiresAt,
            UserId = user.Id,
            Role = user.Role.ToString()
        };
    }
}
