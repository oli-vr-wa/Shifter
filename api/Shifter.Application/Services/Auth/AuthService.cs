using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Shifter.Core.Entities.Identity;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Diagnostics;
using Shifter.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shifter.Application.Interfaces.Repositories.Identity;
using Shifter.Application.DTOs.Identity;
using Shifter.Application.Interfaces.Repositories.Core;
using Shifter.Application.Interfaces.Repositories.CompanyRepos;
using Shifter.Core.Entities.Tenant;

namespace Shifter.Application.Services.Auth;

public class AuthService(
    UserManager<User> userManager, 
    IConfiguration configuration,
    IUserRepository userRepository,
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork) : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <inheritdoc />
    public async Task<IdentityResult> RegisterAsync(RegisterRequest request)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            if (request.Password != request.ConfirmPassword) throw new ArgumentException("Passwords do not match.");

            // Create Company
            var company = new Company
            {
                Name = request.CompanyName,
                Abn = request.CompanyAbn
            };
            await _companyRepository.AddCompanyAsync(company);
            await _unitOfWork.CommitAsync();

            // Create User
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                CompanyId = company.Id,
                Role = Role.CompanyOwner
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) return result;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // TODO: Send the token to the user's email for confirmation. This is a placeholder for actual email sending logic.
            // For now as we do not have email service, we will just log the token to the console for testing purposes.
            Debug.WriteLine($"User Id: {user.Id}");
            Debug.WriteLine($"Email confirmation token for {user.Email}: {token}");

            await transaction.CommitAsync();
            return IdentityResult.Success;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException("An error occurred during registration.", ex);
        }
    }

    /// <inheritdoc />
    public async Task<IdentityResult> ConfirmEmailAsync(Guid userId, string token)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found." });

        var cleanToken = token.Replace(" ", "+");
        var result = await _userManager.ConfirmEmailAsync(user, cleanToken);
        return result;
    }

    /// <inheritdoc />
    public async Task<User?> ValidateUserAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailForAuthAsync(email);
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

        // TODO: Remove this bypass in production. This is only for testing purposes.
        // Bypass MFA verification for testing purposes if the code is "123456"
        if (code == "123456")
        {
            isValid = true;
        }

        if (!isValid) return null;

        // Generate and return the final JWT token
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim("CompanyId", user.CompanyId.ToString()),
        };
        return CreateToken(claims, DateTime.UtcNow.AddDays(1));
    }

    /// <inheritdoc />
    public string GenerateMfaToken(User user)
    {
        var claims = new[]
        {
            new Claim("CompanyId", user.CompanyId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("stage", "mfa_verification")
        };

        return CreateToken(claims, DateTime.UtcNow.AddMinutes(5));
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
