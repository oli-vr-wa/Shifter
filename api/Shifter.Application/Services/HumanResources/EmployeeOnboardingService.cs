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
            var userProfile = new UserProfile
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            var user = new User
            {
                Id = Guid.CreateVersion7(),
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email,
                Role = request.Role,
                EmployeeProfile = userProfile
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
                return ServiceResult<Guid>.Failure(string.Join("; ", createResult.Errors.Select(e => e.Description)));

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
            Console.WriteLine($"Employee User Id: {user.Id} | Password setup token: {passwordSetupToken}");

            await transaction.CommitAsync();            

            return ServiceResult<Guid>.Success(user.Id);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ServiceResult<Guid>> UpdateAsync(Guid employeeId, OnboardEmployeeRequest request)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                return ServiceResult<Guid>.Failure("Employee not found.");
            var user = employee.User;
            if (user == null)
                return ServiceResult<Guid>.Failure("Associated user not found.");
            // Update UserProfile
            var userProfile = user.EmployeeProfile;
            if (userProfile != null)
            {
                userProfile.FirstName = request.FirstName;
                userProfile.LastName = request.LastName;                
            }
            // Update User
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.Role = request.Role;
            await _userManager.UpdateAsync(user);
            // Update Employee
            employee.JobPosition = request.JobPosition;
            employee.EmploymentStartDate = request.EmploymentStartDate;
            await _unitOfWork.CommitAsync();
            await transaction.CommitAsync();

            return ServiceResult<Guid>.Success(employee.Id);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
