using Shifter.Application.DTOs.Timesheet;
using Shifter.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Application.Interfaces.Services.Timesheets;

public interface ITimesheetService
{
    /// <summary>
    /// Starts/clocks in a work event task for a user profile. 
    /// If the work event ID is provided, it will start the task for that specific work event; otherwise, 
    /// it will create a new work event task with the provided title and description.
    /// </summary>
    /// <param name="request">The request containing the details of the work event task to start.</param>
    /// <returns>A ServiceResult indicating the success or failure of the operation.</returns>
    Task<ServiceResult<bool>> StartWorkEventTaskAsync(StartWorkEventTaskRequest request);

    /// <summary>
    /// Ends/clocks out a work event task for a user profile.
    /// </summary>
    /// <param name="request">The request containing the details of the work event task to end.</param>
    /// <returns>A ServiceResult indicating the success or failure of the operation.</returns>
    Task<ServiceResult<bool>> EndWorkEventTaskAsync(EndWorkEventTaskRequest request);
}
