using Shifter.Application.Interfaces.Repositories.Tenant;
using Shifter.Core.Entities.Tenant;
using Shifter.Infrastructure.Data;

namespace Shifter.Infrastructure.Repositories.Tenant;


public class UserProfileRepository(ShifterDbContext dbContext) : IUserProfileRepository
{
    private readonly ShifterDbContext _dbContext = dbContext;

    /// <inheritdoc />
    public async Task Add(UserProfile profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile));
        if (profile.UserId == Guid.Empty) throw new ArgumentException("UserId cannot be empty.", nameof(profile.UserId));
        if (profile.CompanyId == Guid.Empty) throw new ArgumentException("CompanyId cannot be empty.", nameof(profile.CompanyId));

        await _dbContext.EmployeeProfiles.AddAsync(profile);
    }
}
