using Microsoft.EntityFrameworkCore;
using Shifter.Application.Interfaces.Repositories.Identity;
using Shifter.Core.Entities.Identity;
using Shifter.Infrastructure.Data;

namespace Shifter.Infrastructure.Repositories.Identity;

public class UserRepository(ShifterDbContext context) : IUserRepository
{
    private readonly ShifterDbContext _context = context;

    /// <inheritdoc />
    public async Task<User?> GetUserByEmailForAuthAsync(string email)
    {
        return await _context.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    /// <inheritdoc />
    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await _context.Users
            .IgnoreQueryFilters()
            .Include(u => u.EmployeeProfile)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}
