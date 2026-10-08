using Microsoft.EntityFrameworkCore;
using Shifter.Application.DTOs.HumanResources;
using Shifter.Application.Interfaces.Repositories.HumanResources;
using Shifter.Core.Entities.HumanResources;
using Shifter.Infrastructure.Data;

namespace Shifter.Infrastructure.Repositories.HumanResources;

public class EmployeeRepository(ShifterDbContext context) : IEmployeeRepository
{
    private readonly ShifterDbContext _context = context;

    /// <inheritdoc/>
    public async Task AddAsync(Employee employee) =>
        await _context.Employees.AddAsync(employee);

    /// <inheritdoc/>
    public async Task<Employee?> GetByIdAsync(Guid employeeId) =>
        await _context.Employees.FindAsync(employeeId);

    /// <inheritdoc/>
    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        return await _context.Employees
            .Where(e => e.User != null && e.User.EmployeeProfile != null)
            .Select(e => new EmployeeDto(
                e.Id,
                e.User!.EmployeeProfile!.FirstName,
                e.User.EmployeeProfile.LastName,
                e.User.Email!,
                e.User.PhoneNumber,
                e.JobPosition,
                e.EmploymentStartDate,
                e.User.Role
            ))
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<EmployeeDto>> GetAllCurrentAsync()
    {
        return await _context.Employees
            .Where(e => e.User != null && e.User.EmployeeProfile != null && e.EmploymentTerminationDate == null)
            .Select(e => new EmployeeDto(
                e.Id,
                e.User!.EmployeeProfile!.FirstName,
                e.User.EmployeeProfile.LastName,
                e.User.Email!,
                e.User.PhoneNumber,
                e.JobPosition,
                e.EmploymentStartDate,
                e.User.Role
            ))
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<EmployeeDto>> GetAllTerminatedAsync()
    {
        return await _context.Employees
            .Where(e => e.User != null && e.User.EmployeeProfile != null && e.EmploymentTerminationDate != null)
            .Select(e => new EmployeeDto(
                e.Id,
                e.User!.EmployeeProfile!.FirstName,
                e.User.EmployeeProfile.LastName,
                e.User.Email!,
                e.User.PhoneNumber,
                e.JobPosition,
                e.EmploymentStartDate,
                e.User.Role
            ))
            .ToListAsync();
    }
    
}
