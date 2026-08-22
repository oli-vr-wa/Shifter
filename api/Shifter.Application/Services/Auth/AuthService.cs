using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shifter.Application.DTOs.Identity;
using Shifter.Application.Interfaces.Auth;
using Shifter.Application.Interfaces.Repositories.CompanyRepos;
using Shifter.Application.Interfaces.Repositories.Core;
using Shifter.Application.Interfaces.Repositories.Identity;
using Shifter.Application.Interfaces.Repositories.Tenant;
using Shifter.Application.Interfaces.Services.Emails;
using Shifter.Core.Entities.Identity;
using Shifter.Core.Entities.Tenant;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace Shifter.Application.Services.Auth;

public class AuthService(
    UserManager<User> userManager, 
    IConfiguration configuration,
    IUserRepository userRepository,
    IUserProfileRepository userProfileRepository,
    ICompanyRepository companyRepository,
    IEmailService emailService,
    IUnitOfWork unitOfWork) : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserProfileRepository _userProfileRepository = userProfileRepository;
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly IEmailService _emailService = emailService; 
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
                Email = request.Email,
                UserName = request.Email,
                CompanyId = company.Id,
                Role = Role.CompanyOwner
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) return result;

            // Create the UserProfile for the user
            var userProfile = new UserProfile
            {
                UserId = user.Id,
                CompanyId = company.Id,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            await _userProfileRepository.Add(userProfile);
            await _unitOfWork.CommitAsync();    

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token); // Encode the token to make it URL-safe
            var confirmationLink = $"https://localhost:3000/confirm-email?userId={user.Id}&code={encodedToken}";

            Debug.WriteLine($"User Id: {user.Id}");
            Debug.WriteLine($"Email confirmation token for {user.Email}: {token}");

            string subject = "Confirm your Shifter Account";
            string body = $"Welcome to Shifter! Please confirm your account by clicking the following link: <a href='{confirmationLink}'>Confirm Account</a>";

            await _emailService.SendAsync(user.Email, subject, body);

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
