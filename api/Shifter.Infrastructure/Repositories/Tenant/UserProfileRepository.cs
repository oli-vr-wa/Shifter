using Shifter.Application.Interfaces.Repositories.Tenant;
using Shifter.Core.Entities.Tenant;
using Shifter.Infrastructure.Data;

namespace Shifter.Infrastructure.Repositories.Tenant;

public class UserProfileRepository(ShifterDbContext dbContext) : IUserProfileRepository
{
    private readonly ShifterDbContext _dbContext = dbContext;

    /// <inheritdoc />
    public void Add(UserProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile, nameof(profile));
        ArgumentException.ThrowIfNullOrEmpty(profile.UserId.ToString(), nameof(profile.UserId));
        ArgumentException.ThrowIfNullOrEmpty(profile.CompanyId.ToString(), nameof(profile.CompanyId));

        _dbContext.EmployeeProfiles.Add(profile);
    }
}
