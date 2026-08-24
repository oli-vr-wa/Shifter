using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shifter.Application.DTOs.Identity;
using Shifter.Core.Entities.Identity;

namespace Shifter.Application.Interfaces.Auth;

public interface IAuthService
{
    /// <summary>
    /// Registers a new user with the provided registration request and returns the result of the registration process.
    /// </summary>
    /// <param name="request">The registration request containing user details.</param>
    /// <returns>The result of the registration process.</returns>
    Task<IdentityResult> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// Confirms the email of a user with the specified user ID and token, returning the result of the confirmation process.
    /// </summary>
    /// <param name="userId">The ID of the user whose email is to be confirmed.</param>
    /// <param name="token">The email confirmation token.</param>
    /// <returns>The result of the email confirmation process.</returns>
    Task<IdentityResult> ConfirmEmailAsync(Guid userId, string token);

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

    /// <summary>
    /// Checks if multi-factor authentication (MFA) is enabled for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to check if MFA is enabled.</param>
    /// <returns>True if MFA is enabled for the user, otherwise false.</returns>
    Task<bool> IsTwoFactorEnabledAsync(User user);

    /// <summary>
    /// Generates a QR code URI for the specified user and unformatted key, which can be used for setting up multi-factor authentication (MFA) in an authenticator app.
    /// </summary>
    /// <param name="user">The user for whom to generate the QR code URI.</param>
    /// <param name="unformattedKey">The unformatted key to use for generating the QR code URI.</param>
    /// <returns>The generated QR code URI if successful, otherwise null.</returns>
    Task<string?> GenerateQrCodeUri(User user, string? unformattedKey);

    /// <summary>
    /// Retrieves the authenticator key for the specified user, which can be used for setting up multi-factor authentication (MFA) in an authenticator app.
    /// </summary>
    /// <param name="user">The user for whom to retrieve the authenticator key.</param>
    /// <param name="isTwoFactorEnabled">Indicates whether two-factor authentication is enabled for the user.</param>
    /// <returns>The authenticator key if available, otherwise null.</returns>
    Task<string?> GetAuthenticatorKeyAsync(User user, bool isTwoFactorEnabled);

    /// <summary>
    /// Configures and returns the cookie options for authentication cookies, including settings such as expiration, security, and SameSite policy.
    /// </summary>
    /// <returns>The configured cookie options.</returns>
    CookieOptions ConfigureCookieOptions();

    /// <summary>
    /// Retrieves the user response DTO (Data Transfer Object) containing user information such as ID, first name, last name, and email.
    /// </summary>
    /// <returns>The user response DTO.</returns>
    Task<UserResponse?> GetCurrentUserAsync(Guid userId);
}
