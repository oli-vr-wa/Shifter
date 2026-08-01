using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Shifter.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Shifter.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;


namespace Shifter.Application.Services.Auth;

public class AuthService(UserManager<User> userManager, IConfiguration configuration) : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;

    /// <inheritdoc />
    public async Task<User?> ValidateUserAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return null;

        var result = await _userManager.CheckPasswordAsync(user, password);
        return result ? user : null;
    }

    /// <inheritdoc />
    public async Task<string?> VerifyMfaAndGenerateTokenAsync(string userIdStr, string code)
    {
        var user = await _userManager.FindByIdAsync(userIdStr);
        if (user == null) return null;

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, code);

        if (!isValid) return null;

        return GenerateFinalJwtToken(user);
    }

    /// <inheritdoc />
    public string GenerateMfaToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("stage", "mfa_verification")
        };

        return CreateToken(claims, DateTime.UtcNow.AddMinutes(5));
    }

    /// <summary>
    /// Generates a JSON Web Token (JWT) for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate the JWT.</param>
    /// <returns>The generated JWT.</returns>  
    private string GenerateFinalJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim("CompanyId", user.CompanyId.ToString()),
        };
        return CreateToken(claims, DateTime.UtcNow.AddDays(1));
    }

    /// <summary>
    /// Creates a JWT token with the specified claims and expiration time.
    /// </summary>
    /// <param name="claims">The claims to include in the token.</param>
    /// <param name="expires">The expiration time of the token.</param>
    /// <returns>The generated JWT token as a string.</returns>
    private string CreateToken(Claim[] claims, DateTime expires)
    {
        var secretKey = _configuration["Jwt:Secret"];

        if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32) throw new InvalidOperationException("JWT secret is missing or too short. Check your configuration.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
