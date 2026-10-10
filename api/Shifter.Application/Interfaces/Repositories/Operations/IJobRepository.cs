using Shifter.Core.Entities.Operations;

namespace Shifter.Application.Interfaces.Repositories.Operations;

public interface IJobRepository
{
    /// <summary>
    /// Gets a job by its unique identifier asynchronously.
    /// </summary>
    /// <param name="jobId">The unique identifier of the job.</param>
    /// <returns>The job if found; otherwise, null.</returns>
    Task<Job?> GetJobByIdAsync(Guid jobId);

    /// <summary>
    /// Adds a new job to the repository asynchronously.
    /// </summary>
    /// <param name="job">The job to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddJobAsync(Job job);

    /// <summary>
    /// Gets a job by its title asynchronously.
    /// </summary>
    /// <param name="title">The title of the job.</param>
    /// <returns>The job if found; otherwise, null.</returns>
    Task<Job?> GetJobByTitleAsync(string title);

    /// <summary>
    /// Gets all jobs asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of all jobs.</returns>
    Task<IEnumerable<Job>> GetAllJobsAsync();

    /// <summary>
    /// Gets all jobs for a specific client asynchronously.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of jobs for the specified client.</returns>
    Task<IEnumerable<Job>> GetJobsByClientIdAsync(Guid clientId);

    /// <summary>
    /// Gets all uncompleted jobs asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of uncompleted jobs.</returns>
    Task<IEnumerable<Job>> GetUncompletedJobsAsync();
}
