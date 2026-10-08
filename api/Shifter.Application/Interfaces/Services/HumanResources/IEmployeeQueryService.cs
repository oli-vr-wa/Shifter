using Shifter.Application.DTOs.HumanResources;

namespace Shifter.Application.Interfaces.Services.HumanResources;

public interface IEmployeeQueryService
{
    /// <summary>
    /// Gets all employees asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="EmployeeDto"/>.</returns>
    Task<IEnumerable<EmployeeDto>> GetAllAsync();

    /// <summary>
    /// Gets all current employees asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="EmployeeDto"/>.</returns>
    Task<IEnumerable<EmployeeDto>> GetAllCurrentAsync();

    /// <summary>
    /// Gets all terminated employees asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="EmployeeDto"/>.</returns>
    Task<IEnumerable<EmployeeDto>> GetAllTerminatedAsync();

    /// <summary>
    /// Gets an employee by their ID asynchronously.
    /// </summary>
    /// <param name="employeeId">The ID of the employee to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="EmployeeDto"/> if found; otherwise, null.</returns>
    Task<EmployeeDto?> GetByIdAsync(Guid employeeId);
}
