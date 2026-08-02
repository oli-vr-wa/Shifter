using Shifter.Core.Entities.Tenant;

namespace Shifter.Application.Interfaces.Repositories.Tenant;

public interface IUserProfileRepository
{
    /// <summary>
    /// Adds a new user profile to the repository.
    /// This method does not commit changes to the database; it only adds the profile to the context.
    /// </summary>
    /// <param name="profile">The user profile to add.</param>
    Task Add(UserProfile profile);
}
