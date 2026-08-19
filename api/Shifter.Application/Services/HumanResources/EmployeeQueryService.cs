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
}
