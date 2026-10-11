using Shifter.Application.DTOs.HumanResources;

namespace Shifter.Application.Interfaces.Services.HumanResources;

public interface IEmployeeQueryService
{
    /// <summary>
    /// Gets all employees asynchronously.
    /// </summary>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="EmployeeDto"/>.</returns>
    Task<IEnumerable<EmployeeDto>> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets all current employees asynchronously.
    /// </summary>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="EmployeeDto"/>.</returns>
    Task<IEnumerable<EmployeeDto>> GetAllCurrentAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets all terminated employees asynchronously.
    /// </summary>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="EmployeeDto"/>.</returns>
    Task<IEnumerable<EmployeeDto>> GetAllTerminatedAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets an employee by their ID asynchronously.
    /// </summary>
    /// <param name="employeeId">The ID of the employee to retrieve.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="EmployeeDto"/> if found; otherwise, null.</returns>
    Task<EmployeeDto?> GetByIdAsync(Guid employeeId, CancellationToken ct = default);
}
