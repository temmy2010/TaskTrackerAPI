using Microsoft.EntityFrameworkCore;
using TaskTrackerAPI.Domain.Entities;
using TaskTrackerAPI.Domain.Interfaces;
using TaskTrackerAPI.Infrastructure.Data;
using TaskTrackerAPI.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsername(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
