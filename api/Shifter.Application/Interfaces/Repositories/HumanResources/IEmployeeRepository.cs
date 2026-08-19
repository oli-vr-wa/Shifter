using Shifter.Application.DTOs.HumanResources;
using Shifter.Core.Entities.HumanResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Application.Interfaces.Repositories.HumanResources;

public interface IEmployeeRepository
{
    /// <summary>
    /// Retrieves an Employee by its unique identifier asynchronously.
    /// </summary>
    /// <param name="employeeId">The unique identifier of the Employee to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Employee if found; otherwise, null.</returns>
    Task<Employee?> GetByIdAsync(Guid employeeId);

    /// <summary>
    /// Adds a new Employee to the repository asynchronously.
    /// </summary>
    /// <param name="employee">The Employee to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(Employee employee);

    /// <summary>
    /// Retrieves all Employees from the repository asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of EmployeeDto.</returns>
    Task<IEnumerable<EmployeeDto>> GetAllAsync();

    /// <summary>
    /// Retrieves all current Employees from the repository asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of EmployeeDto.</returns>
    Task<IEnumerable<EmployeeDto>> GetAllCurrentAsync();

    /// <summary>
    /// Retrieves all terminated Employees from the repository asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of EmployeeDto.</returns> 
    Task<IEnumerable<EmployeeDto>> GetAllTerminatedAsync();
}
