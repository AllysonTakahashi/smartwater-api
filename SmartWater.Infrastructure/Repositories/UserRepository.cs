using Microsoft.EntityFrameworkCore;
using SmartWater.Application.Interfaces;
using SmartWater.Domain.Entities;
using SmartWater.Infrastructure.Data;

namespace SmartWater.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<User?> FindByUsernameAsync(string username)
        => _context.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}
