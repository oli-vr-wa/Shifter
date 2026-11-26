using Shifter.Application.DTOs.Operations;
using Shifter.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Application.Interfaces.Services.Operations;

public interface IJobCommandService
{
    /// <summary>
    /// Creates a job and returns the created job identifier in a service result.
    /// </summary>
    /// <param name="jobDto">Job data to create.</param>
    /// <param name="ct">Token used to propagate notification that the operation should be canceled.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created job identifier as a <see
    /// cref="Guid"/> in a <see cref="ServiceResult{T}"/>.</returns>
    Task<ServiceResult<Guid>> CreateAsync(JobDto jobDto, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing job identified by its unique identifier.
    /// </summary>
    /// <param name="guid">Unique identifier of the job to update.</param>
    /// <param name="jobDto">Job data to apply to the existing job.</param>
    /// <param name="ct">Token used to cancel the asynchronous operation.</param>
    /// <returns>A task that resolves to a service result containing <c>true</c> when the update succeeds; otherwise,
    /// <c>false</c>.</returns>
    Task<ServiceResult<bool>> UpdateAsync(Guid guid, JobDto jobDto, CancellationToken ct = default);
}
