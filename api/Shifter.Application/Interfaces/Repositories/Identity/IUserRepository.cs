using Shifter.Core.Entities.Identity;

namespace Shifter.Application.Interfaces.Repositories.Identity;

public interface IUserRepository
{
    /// <summary>
    /// Gets a user by email for authentication purposes bypassing the tenant filter. 
    /// This is used for login and password reset scenarios where the query needs to skip the tenant filter as user not logged in yet.
    /// </summary>
    /// <param name="email">The email of the user to retrieve.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    Task<User?> GetUserByEmailForAuthAsync(string email);

    /// <summary>
    /// Gets a user by their unique identifier (userId).
    /// </summary>
    /// <param name="userId">The unique identifier of the user to retrieve.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    Task<User?> GetUserByIdAsync(Guid userId);
}
