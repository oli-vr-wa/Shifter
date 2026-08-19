using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Shifter.Application.DTOs.HumanResources;
using Shifter.Application.Interfaces.Repositories.Core;
using Shifter.Application.Interfaces.Repositories.HumanResources;
using Shifter.Application.Interfaces.Repositories.Tenant;
using Shifter.Application.Interfaces.Services.HumanResources;
using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.HumanResources;
using Shifter.Core.Entities.Identity;
using Shifter.Core.Entities.Tenant;

namespace Shifter.Application.Services.HumanResources;

public class EmployeeOnboardingService(
    UserManager<User> userManager,
    IUserProfileRepository userProfileRepository,
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork) : IEmployeeOnboardingService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IUserProfileRepository _userProfileRepository = userProfileRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <inheritdoc />
    public async Task<ServiceResult<Guid>> OnboardAsync(OnboardEmployeeRequest request)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var user = new User
            {
                Email = request.Email,
                UserName = request.Email,
                Role = Role.Employee
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
                return ServiceResult<Guid>.Failure(string.Join("; ", createResult.Errors.Select(e => e.Description)));

            var userProfile = new UserProfile
            {
                UserId = user.Id,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            await _userProfileRepository.Add(userProfile);

            var employee = new Employee
            {
                UserId = user.Id,
                JobPosition = request.JobPosition,
                EmploymentStartDate = request.EmploymentStartDate
            };
            await _employeeRepository.AddAsync(employee);

            await _unitOfWork.CommitAsync();

            var passwordSetupToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // TODO: Send passwordSetupToken to the employee's email so they can set up their password.
            Debug.WriteLine($"Employee User Id: {user.Id} | Password setup token: {passwordSetupToken}");

            await transaction.CommitAsync();

            return ServiceResult<Guid>.Success(user.Id);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
