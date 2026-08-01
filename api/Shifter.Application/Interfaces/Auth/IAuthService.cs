using Shifter.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Application.Interfaces.Auth;

public interface IAuthService
{
    /// <summary>
    /// Validates the user credentials and returns the user if valid, otherwise returns null.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>The user if the credentials are valid, otherwise null.</returns>
    Task<User?> ValidateUserAsync(string email, string password);

    /// <summary>
    /// Verifies the multi-factor authentication (MFA) code for the specified user and generates a JWT token if the verification is successful.
    /// </summary>
    /// <param name="userIdStr">The ID of the user as a string.</param>
    /// <param name="code">The MFA code to verify.</param>
    /// <returns>The generated JWT token if the verification is successful, otherwise null.</returns>
    Task<string?> VerifyMfaAndGenerateTokenAsync(string userIdStr, string code);

    /// <summary>
    /// Generates a multi-factor authentication (MFA) token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate the MFA token.</param>
    /// <returns>The generated MFA token.</returns>
    string GenerateMfaToken(User user);    
}
