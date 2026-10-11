using Shifter.Application.DTOs.HumanResources;
using Shifter.Application.Interfaces.Repositories.HumanResources;
using Shifter.Application.Interfaces.Services.HumanResources;

namespace Shifter.Application.Services.HumanResources;

public class EmployeeQueryService(IEmployeeRepository employeeRepository) : IEmployeeQueryService
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;

    /// <inheritdoc/>
    public async Task<IEnumerable<EmployeeDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _employeeRepository.GetAllAsync(ct);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<EmployeeDto>> GetAllCurrentAsync(CancellationToken ct = default)
    {
        return await _employeeRepository.GetAllCurrentAsync(ct);
    }
    
    /// <inheritdoc/>
    public async Task<IEnumerable<EmployeeDto>> GetAllTerminatedAsync(CancellationToken ct = default)
    {
        return await _employeeRepository.GetAllTerminatedAsync(ct);
    }

    /// <inheritdoc/>
    public async Task<EmployeeDto?> GetByIdAsync(Guid employeeId, CancellationToken ct = default)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId, ct);
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
