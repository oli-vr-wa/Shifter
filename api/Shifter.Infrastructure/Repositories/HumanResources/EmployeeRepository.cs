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
    public void Add(Employee employee) =>
        _context.Employees.Add(employee);

    /// <inheritdoc/>
    public async Task<Employee?> GetByIdAsync(Guid employeeId, CancellationToken ct = default) =>
        await _context.Employees
            .Include(e => e.User)
            .ThenInclude(e => e!.EmployeeProfile)
            .FirstOrDefaultAsync(e => e.Id == employeeId, ct);

    /// <inheritdoc/>
    public async Task<IEnumerable<EmployeeDto>> GetAllAsync(CancellationToken ct = default)
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
            .ToListAsync(ct);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<EmployeeDto>> GetAllCurrentAsync(CancellationToken ct = default)
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
            .ToListAsync(ct);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<EmployeeDto>> GetAllTerminatedAsync(CancellationToken ct = default)
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
            .ToListAsync(ct);
    }
    
}
