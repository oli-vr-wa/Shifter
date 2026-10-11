using Shifter.Application.DTOs.Location;
using Shifter.Core.Entities.Location;
using Shifter.Core.Entities.Timesheets.Enums;

namespace Shifter.Application.Interfaces.Services.Timesheets;

public interface IWorkEventHistoryLogHandler
{
    /// <summary>
    /// Adds a work event history log entry to the database.
    /// </summary>
    /// <param name="workEventId">The ID of the work event.</param>
    /// <param name="status">The status of the work event.</param>
    /// <param name="addressLocation">The address location where the action was performed, if applicable.</param>
    void AddWorkEventHistoryLog(Guid workEventId, WorkEventStatus status, Address? addressLocation);

    /// <summary>
    /// Adds a break event history log entry to the database.
    /// </summary>
    /// <param name="breakEventId">The ID of the break event.</param>
    /// <param name="status">The status of the break event.</param>
    /// <param name="addressLocation">The address location where the action was performed, if applicable.</param>
    void AddBreakEventHistoryLog(Guid breakEventId, WorkEventStatus status, Address? addressLocation);
}
