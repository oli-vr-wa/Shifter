using Shifter.Application.DTOs.HumanResources;
using Shifter.Application.Interfaces.Repositories.HumanResources;
using Shifter.Application.Interfaces.Services.HumanResources;

namespace Shifter.Application.Services.HumanResources;

public class EmployeeQueryService(IEmployeeRepository employeeRepository) : IEmployeeQueryService
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;

    /// <inheritdoc/>
    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        return await _employeeRepository.GetAllAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<EmployeeDto>> GetAllCurrentAsync()
    {
        return await _employeeRepository.GetAllCurrentAsync();
    }
    
    /// <inheritdoc/>
    public async Task<IEnumerable<EmployeeDto>> GetAllTerminatedAsync()
    {
        return await _employeeRepository.GetAllTerminatedAsync();
    }

    /// <inheritdoc/>
    public async Task<EmployeeDto?> GetByIdAsync(Guid employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee == null || employee.User == null || employee.User.EmployeeProfile == null)
        {
            return null;
        }
        return new EmployeeDto(
            employee.Id,
            employee.User.EmployeeProfile.FirstName,
            employee.User.EmployeeProfile.LastName,
            employee.User.Email!,
            employee.User.PhoneNumber,
            employee.JobPosition,
            employee.EmploymentStartDate,
            employee.User.Role
        );
    }
}
