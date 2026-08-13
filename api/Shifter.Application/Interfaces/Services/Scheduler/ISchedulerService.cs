using Shifter.Application.DTOs.Scheduler;
using Shifter.Core.Entities.Common;

namespace Shifter.Application.Interfaces.Services.Scheduler;

public interface ISchedulerService
{
    /// <summary>
    /// Schedules a work event for a user profile.
    /// </summary>
    /// <param name="request">The request containing the details of the work event to be scheduled.</param>
    /// <returns>A ServiceResult indicating the success or failure of the operation.</returns>
    Task<ServiceResult<bool>> ScheduleWorkEventAsync(ScheduleWorkEventRequest request);
}
