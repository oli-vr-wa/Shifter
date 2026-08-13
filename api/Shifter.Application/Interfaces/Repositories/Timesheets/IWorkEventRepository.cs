using Shifter.Core.Entities.Timesheets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Application.Interfaces.Repositories.Timesheets;

public interface IWorkEventRepository
{
    /// <summary>
    /// Adds a new WorkEvent to the repository asynchronously.
    /// </summary>
    /// <param name="workEvent">The WorkEvent to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(WorkEvent workEvent);

    /// <summary>
    /// Retrieves a WorkEvent by its unique identifier asynchronously.
    /// </summary>
    /// <param name="workEventId">The unique identifier of the WorkEvent to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the WorkEvent if found; otherwise, null.</returns>
    Task<WorkEvent?> GetByIdAsync(Guid workEventId);
}
