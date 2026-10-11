using Shifter.Core.Entities.Timesheets;

namespace Shifter.Application.Interfaces.Repositories.Timesheets;

public interface IWorkEventRepository
{
    /// <summary>
    /// Adds a new WorkEvent to the repository.
    /// </summary>
    /// <param name="workEvent">The WorkEvent to add.</param>
    void Add(WorkEvent workEvent);

    /// <summary>
    /// Retrieves a WorkEvent by its unique identifier asynchronously.
    /// </summary>
    /// <param name="workEventId">The unique identifier of the WorkEvent to retrieve.</param>
    /// <param name="ct">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the WorkEvent if found; otherwise, null.</returns>
    Task<WorkEvent?> GetByIdAsync(Guid workEventId, CancellationToken ct = default);
}
