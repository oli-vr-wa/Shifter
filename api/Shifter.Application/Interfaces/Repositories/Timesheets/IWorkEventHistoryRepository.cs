using Shifter.Core.Entities.Timesheets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Application.Interfaces.Repositories.Timesheets;

public interface IWorkEventHistoryRepository
{
    /// <summary>
    /// Adds a new work event history record to the repository asynchronously.
    /// </summary>
    /// <param name="workEventHistory">The work event history record to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(WorkEventHistory workEventHistory);
}
