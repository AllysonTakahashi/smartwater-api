using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartWater.Application.Exceptions;
using SmartWater.Application.Interfaces;
using SmartWater.Application.Models;
using SmartWater.Application.Settings;
using SmartWater.Domain.Entities;

namespace SmartWater.Application.Services;

public class AuthService : IAuthService
{
    private static readonly JwtSecurityTokenHandler TokenHandler = new();
    private const int BcryptWorkFactor = 12;

    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        IOptions<JwtSettings> jwtOptions,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtOptions.Value;
        _logger = logger;
    }

    public async Task RegisterAsync(string username, string password)
    {
        var existing = await _userRepository.FindByUsernameAsync(username);
        if (existing is not null)
        {
            _logger.LogWarning("Registration failed: username '{Username}' is already taken.", username);
            throw new DuplicateUsernameException(username);
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, BcryptWorkFactor);
        var user = new User(username, passwordHash);

        await _userRepository.AddAsync(user);
        _logger.LogInformation("User '{Username}' registered successfully.", username);
    }

    public async Task<TokenResult?> LoginAsync(string username, string password)
    {
        var user = await _userRepository.FindByUsernameAsync(username);

        if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            _logger.LogWarning("Failed login attempt for username '{Username}'.", username);
            return null;
        }

        _logger.LogInformation("User '{Username}' logged in successfully.", username);
        return BuildToken(user);
    }

    private TokenResult BuildToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new TokenResult(TokenHandler.WriteToken(token), expiresAt);
    }
}
