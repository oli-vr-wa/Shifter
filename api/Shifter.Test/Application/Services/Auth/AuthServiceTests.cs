using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shifter.Application.DTOs.Identity;
using Shifter.Application.Interfaces.Repositories.CompanyRepos;
using Shifter.Application.Interfaces.Repositories.Core;
using Shifter.Application.Interfaces.Repositories.HumanResources;
using Shifter.Application.Interfaces.Repositories.Identity;
using Shifter.Application.Interfaces.Services.Emails;
using Shifter.Application.Interfaces.Tenant.Handlers;
using Shifter.Application.Services.Auth;
using Shifter.Core.Entities.HumanResources;
using Shifter.Core.Entities.Identity;
using Shifter.Core.Entities.Tenant;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Shifter.Test.Application.Services.Auth;

public class AuthServiceTests
{
    private readonly ICompanyHandler _companyHandler;
    private readonly ICompanyRepository _companyRepository;
    private readonly IEmailService _emailService;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContextTransaction _transaction;
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _companyHandler = Substitute.For<ICompanyHandler>();
        _companyRepository = Substitute.For<ICompanyRepository>();
        _emailService = Substitute.For<IEmailService>();
        _employeeRepository = Substitute.For<IEmployeeRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _transaction = Substitute.For<IDbContextTransaction>();
        _userRepository = Substitute.For<IUserRepository>();

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Secret"] = "12345678901234567890123456789012",
                ["Jwt:Issuer"] = "https://localhost:5001",
                ["Jwt:Audience"] = "Shifter"
            })
            .Build();

        var identityOptions = Options.Create(new IdentityOptions());
        var services = new ServiceCollection().BuildServiceProvider();
        var userStore = Substitute.For<IUserStore<User>>();

        _userManager = Substitute.For<UserManager<User>>(
            userStore,
            identityOptions,
            new PasswordHasher<User>(),
            new List<IUserValidator<User>>(),
            new List<IPasswordValidator<User>>(),
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            services,
            Substitute.For<ILogger<UserManager<User>>>());

        _unitOfWork.BeginTransactionAsync(Arg.Any<CancellationToken>()).Returns(_transaction);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(1);
        _transaction.CommitAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _transaction.RollbackAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        _sut = new AuthService(
            _userManager,
            _configuration,
            _userRepository,
            _companyRepository,
            _employeeRepository,
            _companyHandler,
            _emailService,
            _unitOfWork);
    }

    [Fact]
    public async Task RegisterAsync_WhenPasswordsDoNotMatch_ThrowsInvalidOperationException()
    {
        var request = new RegisterRequest(
            "Jane",
            "Doe",
            "jane@example.com",
            "Password!123",
            "Password!456",
            "Shifter Labs",
            "123456789");

        var act = async () => await _sut.RegisterAsync(request, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("An error occurred during registration.");

        await _transaction.Received(1).RollbackAsync(Arg.Any<CancellationToken>());
        _companyRepository.DidNotReceive().Add(Arg.Any<Company>());
        await _userManager.DidNotReceive().CreateAsync(Arg.Any<User>(), Arg.Any<string>());
    }

    [Fact]
    public async Task RegisterAsync_WhenUserCreationFails_ReturnsFailureWithoutSendingEmail()
    {
        var request = new RegisterRequest(
            "Jane",
            "Doe",
            "jane@example.com",
            "Password!123",
            "Password!123",
            "Shifter Labs",
            "123456789");

        _companyHandler.FormatAbn(request.CompanyAbn).Returns("123456789");
        _userManager.CreateAsync(Arg.Any<User>(), request.Password)
            .Returns(IdentityResult.Failed(new IdentityError { Description = "User already exists." }));

        var result = await _sut.RegisterAsync(request, CancellationToken.None);

        result.Succeeded.Should().BeFalse();
        await _emailService.DidNotReceive().SendAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
        _employeeRepository.DidNotReceive().Add(Arg.Any<Employee>());
    }

    [Fact]
    public async Task RegisterAsync_WhenRegistrationSucceeds_CreatesCompanyAndEmployeeAndSendsConfirmationEmail()
    {
        var request = new RegisterRequest(
            "Jane",
            "Doe",
            "jane@example.com",
            "Password!123",
            "Password!123",
            "Shifter Labs",
            "123456789");

        Company? createdCompany = null;
        User? createdUser = null;

        _companyHandler.FormatAbn(request.CompanyAbn).Returns("123456789");
        _userManager.CreateAsync(Arg.Do<User>(user => createdUser = user), request.Password)
            .Returns(IdentityResult.Success);
        _userManager.GenerateEmailConfirmationTokenAsync(Arg.Any<User>()).Returns("confirmation-token");
        _companyRepository
            .When(x => x.Add(Arg.Any<Company>()))
            .Do(call => createdCompany = call.Arg<Company>());

        var result = await _sut.RegisterAsync(request, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
        createdCompany.Should().NotBeNull();
        createdCompany!.Name.Should().Be(request.CompanyName);
        createdCompany.Abn.Should().Be("123456789");
        createdUser.Should().NotBeNull();
        createdUser!.Email.Should().Be(request.Email);
        createdUser.UserName.Should().Be(request.Email);
        createdUser.Role.Should().Be(Role.CompanyOwner);

        _employeeRepository.Received(1).Add(Arg.Is<Employee>(employee =>
            employee.UserId == createdUser.Id &&
            employee.CompanyId == createdCompany.Id));

        await _emailService.Received(1).SendAsync(
            request.Email,
            "Confirm your Shifter Account",
            Arg.Is<string>(body => body.Contains("Confirm Account", StringComparison.Ordinal)),
            Arg.Any<CancellationToken>());

        await _transaction.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ConfirmEmailAsync_WhenUserNotFound_ReturnsFailedIdentityResult()
    {
        var userId = Guid.NewGuid();
        _userRepository.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns((User?)null);

        var result = await _sut.ConfirmEmailAsync(userId, "token-value", CancellationToken.None);

        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.Description == "User not found.");
    }

    [Fact]
    public async Task ValidateUserAsync_WhenCredentialsAreValid_ReturnsUser()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "jane@example.com",
            UserName = "jane@example.com"
        };

        _userRepository.GetUserByEmailForAuthAsync(user.Email!, Arg.Any<CancellationToken>())
            .Returns(user);
        _userManager.CheckPasswordAsync(user, "Password!123").Returns(true);

        var result = await _sut.ValidateUserAsync(user.Email!, "Password!123");

        result.Should().BeSameAs(user);
    }

    [Fact]
    public async Task VerifyMfaAndGenerateTokenAsync_WhenCodeIsValid_GeneratesJwtWithAuthenticatedClaims()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "jane@example.com",
            CompanyId = Guid.NewGuid()
        };

        _userManager.FindByIdAsync(user.Id.ToString()).Returns(user);
        _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, "123456")
            .Returns(true);
        _userManager.GetTwoFactorEnabledAsync(user).Returns(false);
        _userManager.SetTwoFactorEnabledAsync(user, true).Returns(IdentityResult.Success);

        var token = await _sut.VerifyMfaAndGenerateTokenAsync(user.Id.ToString(), "123456");

        token.Should().NotBeNullOrWhiteSpace();

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token!);
        jwt.Claims.Should().Contain(claim => claim.Type == ClaimTypes.NameIdentifier && claim.Value == user.Id.ToString());
        jwt.Claims.Should().Contain(claim => claim.Type == ClaimTypes.Email && claim.Value == user.Email);
        jwt.Claims.Should().Contain(claim => claim.Type == "CompanyId" && claim.Value == user.CompanyId.ToString());
        jwt.Claims.Should().Contain(claim => claim.Type == "stage" && claim.Value == "fully_authenticated");
    }

    [Fact]
    public void GenerateMfaToken_WhenCalled_GeneratesJwtWithMfaVerificationStage()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "jane@example.com",
            CompanyId = Guid.NewGuid()
        };

        var token = _sut.GenerateMfaToken(user);

        token.Should().NotBeNullOrWhiteSpace();

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        jwt.Claims.Should().Contain(claim => claim.Type == "CompanyId" && claim.Value == user.CompanyId.ToString());
        jwt.Claims.Should().Contain(claim => claim.Type == ClaimTypes.NameIdentifier && claim.Value == user.Id.ToString());
        jwt.Claims.Should().Contain(claim => claim.Type == "stage" && claim.Value == "mfa_verification");
    }

    [Fact]
    public async Task GetCurrentUserAsync_WhenUserExists_ReturnsMappedUserResponse()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "jane@example.com",
            EmployeeProfile = new UserProfile
            {
                FirstName = "Jane",
                LastName = "Doe"
            }
        };

        _userRepository.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);

        var result = await _sut.GetCurrentUserAsync(userId, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
        result.FirstName.Should().Be("Jane");
        result.LastName.Should().Be("Doe");
        result.Email.Should().Be("jane@example.com");
    }
}
