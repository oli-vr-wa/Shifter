using Shifter.Application.Interfaces.Repositories.Timesheets;
using Shifter.Core.Entities.Timesheets;
using Shifter.Infrastructure.Data;

namespace Shifter.Infrastructure.Repositories.Timesheets;

public class WorkEventRepository(ShifterDbContext context) : IWorkEventRepository
{
    private readonly ShifterDbContext _context = context;

    /// <inheritdoc/>
    public void Add(WorkEvent workEvent)
    {
        _context.WorkEvents.Add(workEvent);
    }

    /// <inheritdoc/>
    public async Task<WorkEvent?> GetByIdAsync(Guid workEventId, CancellationToken ct = default)
    {
        return await _context.WorkEvents.FindAsync([workEventId], ct);
    }
}
