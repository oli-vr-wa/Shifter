using Shifter.Core.Entities.Timesheets;

namespace Shifter.Application.Interfaces.Repositories.Timesheets;

public interface IBreakEventRepository
{
    /// <summary>
    /// Adds a new break event to the repository asynchronously.
    /// </summary>
    /// <param name="breakEvent">The BreakEvent to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(BreakEvent breakEvent);
}
