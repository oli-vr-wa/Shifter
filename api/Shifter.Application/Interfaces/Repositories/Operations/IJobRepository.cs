using Shifter.Core.Entities.Operations;

namespace Shifter.Application.Interfaces.Repositories.Operations;

public interface IJobRepository
{
    /// <summary>
    /// Gets a job by its unique identifier asynchronously.
    /// </summary>
    /// <param name="jobId">The unique identifier of the job.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The job if found; otherwise, null.</returns>
    Task<Job?> GetByIdAsync(Guid jobId, CancellationToken ct = default);

    /// <summary>
    /// Adds a new job to the repository asynchronously.
    /// </summary>
    /// <param name="job">The job to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    void Add(Job job);

    /// <summary>
    /// Gets a job by its title asynchronously.
    /// </summary>
    /// <param name="title">The title of the job.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The job if found; otherwise, null.</returns>
    Task<Job?> GetByTitleAsync(string title, CancellationToken ct = default);

    /// <summary>
    /// Gets all jobs asynchronously.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of all jobs.</returns>
    Task<IEnumerable<Job>> GetAllJobsAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets all jobs for a specific client asynchronously.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of jobs for the specified client.</returns>
    Task<IEnumerable<Job>> GetJobsByClientIdAsync(Guid clientId, CancellationToken ct = default);

    /// <summary>
    /// Gets all uncompleted jobs asynchronously.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of uncompleted jobs.</returns>
    Task<IEnumerable<Job>> GetUncompletedJobsAsync(CancellationToken ct = default);
}
