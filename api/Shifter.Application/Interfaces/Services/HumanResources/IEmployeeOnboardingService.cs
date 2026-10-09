using Shifter.Application.DTOs.HumanResources;
using Shifter.Core.Entities.Common;

namespace Shifter.Application.Interfaces.Services.HumanResources;

public interface IEmployeeOnboardingService
{
    /// <summary>
    /// Onboards a new employee by creating a User (without a password), a UserProfile, and an Employee record.
    /// The employee will receive a link to set up their password.
    /// </summary>
    /// <param name="request">The information required to onboard the new employee.</param>
    /// <returns>A <see cref="ServiceResult{T}"/> containing the employee's user ID on success.</returns>
    Task<ServiceResult<Guid>> OnboardAsync(OnboardEmployeeRequest request);

    /// <summary>
    /// Updates an existing employee's information.
    /// </summary>
    /// <param name="employeeId">The ID of the employee to update.</param>
    /// <param name="request">The updated employee information.</param>
    /// <returns>A <see cref="ServiceResult{T}"/> indicating the success or failure of the operation.</returns>
    Task<ServiceResult<Guid>> UpdateAsync(Guid employeeId, OnboardEmployeeRequest request);
}
