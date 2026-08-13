using Shifter.Application.Interfaces.Repositories.Timesheets;
using Shifter.Core.Entities.Timesheets;
using Shifter.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Infrastructure.Repositories.Timesheets;

public class WorkEventHistoryRepository(ShifterDbContext dbContext) : IWorkEventHistoryRepository
{
    private readonly ShifterDbContext _dbContext = dbContext;

    /// <inheritdoc />
    public async Task AddAsync(WorkEventHistory workEventHistory) =>
        await _dbContext.WorkEventHistories.AddAsync(workEventHistory);
}
