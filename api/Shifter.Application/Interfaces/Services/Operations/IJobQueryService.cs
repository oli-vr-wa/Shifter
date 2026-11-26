using Shifter.Application.DTOs.Common;
using Shifter.Application.DTOs.Operations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Application.Interfaces.Services.Operations;

public interface IJobQueryService
{
    /// <summary>
    /// Asynchronously retrieves a job by its unique identifier.
    /// </summary>
    /// <param name="jobId">The unique identifier of the job to retrieve.</param>
    /// <param name="ct">A token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the matching job if found;
    /// otherwise, <see langword="null"/>.</returns>
    Task<JobDto?> GetByIdAsync(Guid jobId, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously retrieves a paged collection of jobs that match the specified query filter.
    /// </summary>
    /// <param name="filter">The query criteria used to filter the jobs to retrieve.</param>
    /// <param name="ct">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paged job items that match the
    /// filter.</returns>
    Task<PageResult<JobListItemDto>> GetAllAsync(JobQueryFilter filter, CancellationToken ct = default);

    /// <summary>
    /// Gets a paged list of jobs for the specified client.
    /// </summary>
    /// <param name="clientId">Unique identifier of the client.</param>
    /// <param name="filter">Query criteria and paging options to apply.</param>
    /// <param name="ct">Token used to cancel the operation.</param>
    /// <returns>A task that resolves to a page of <see cref="JobListItemDto"/> results matching the client and filter.</returns>
    Task<PageResult<JobListItemDto>> GetByClientIdAsync(Guid clientId, JobQueryFilter filter, CancellationToken ct = default);
}
