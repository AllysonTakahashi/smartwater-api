using SmartWater.Application.Models;

namespace SmartWater.Application.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(string username, string password);
    Task<TokenResult?> LoginAsync(string username, string password);
}
