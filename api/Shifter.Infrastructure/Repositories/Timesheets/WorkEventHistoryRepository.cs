using Shifter.Application.Interfaces.Repositories.Timesheets;
using Shifter.Core.Entities.Timesheets;
using Shifter.Infrastructure.Data;

namespace Shifter.Infrastructure.Repositories.Timesheets;

public class WorkEventHistoryRepository(ShifterDbContext dbContext) : IWorkEventHistoryRepository
{
    private readonly ShifterDbContext _dbContext = dbContext;

    /// <inheritdoc />
    public void Add(WorkEventHistory workEventHistory) => _dbContext.WorkEventHistories.Add(workEventHistory);
}
