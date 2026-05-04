using SmartWater.Domain.Entities;

namespace SmartWater.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> FindByUsernameAsync(string username);
    Task AddAsync(User user);
}
