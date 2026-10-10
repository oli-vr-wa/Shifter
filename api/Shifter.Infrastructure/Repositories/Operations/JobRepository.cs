using Microsoft.EntityFrameworkCore;
using Shifter.Application.Interfaces.Repositories.Operations;
using Shifter.Core.Entities.Operations;
using Shifter.Infrastructure.Data;

namespace Shifter.Infrastructure.Repositories.Operations;

public class JobRepository(ShifterDbContext dbContext) : IJobRepository
{
    private readonly ShifterDbContext _dbContext = dbContext;

    /// <inheritdoc />
    public async Task AddJobAsync(Job job)
    {
        if (job == null) throw new ArgumentNullException(nameof(job), "Job cannot be null.");
        if (string.IsNullOrWhiteSpace(job.Title)) throw new ArgumentException("Job title cannot be null or empty.", nameof(job.Title));
        
        await _dbContext.Jobs.AddAsync(job);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Job>> GetAllJobsAsync()
    {
        return await _dbContext.Jobs.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Job?> GetJobByIdAsync(Guid jobId)
    {
        return await _dbContext.Jobs.FindAsync(jobId);
    }

    /// <inheritdoc />
    public async Task<Job?> GetJobByTitleAsync(string title)
    {
        return await _dbContext.Jobs.FirstOrDefaultAsync(j => j.Title == title);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Job>> GetJobsByClientIdAsync(Guid clientId)
    {
        return await _dbContext.Jobs.Where(j => j.ClientId == clientId).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Job>> GetUncompletedJobsAsync()
    {
        return await _dbContext.Jobs.Where(m => m.Status != JobStatus.Completed && m.Status != JobStatus.Cancelled).ToListAsync();
    }
}
