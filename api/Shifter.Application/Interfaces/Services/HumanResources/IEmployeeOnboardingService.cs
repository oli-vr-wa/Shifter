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
}
