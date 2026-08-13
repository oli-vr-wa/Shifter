using Shifter.Application.Interfaces.Repositories.Timesheets;
using Shifter.Core.Entities.Timesheets;
using Shifter.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Infrastructure.Repositories.Timesheets;

public class WorkEventRepository(ShifterDbContext context) : IWorkEventRepository
{
    private readonly ShifterDbContext _context = context;

    /// <inheritdoc/>
    public async Task AddAsync(WorkEvent workEvent)
    {
        await _context.WorkEvents.AddAsync(workEvent);
    }

    /// <inheritdoc/>
    public async Task<WorkEvent?> GetByIdAsync(Guid workEventId)
    {
        return await _context.WorkEvents.FindAsync(workEventId);
    }
}
