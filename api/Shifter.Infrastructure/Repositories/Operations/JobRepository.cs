using Microsoft.EntityFrameworkCore;
using Shifter.Application.Interfaces.Repositories.Operations;
using Shifter.Core.Entities.Operations;
using Shifter.Infrastructure.Data;

namespace Shifter.Infrastructure.Repositories.Operations;

public class JobRepository(ShifterDbContext dbContext) : IJobRepository
{
    private readonly ShifterDbContext _dbContext = dbContext;

    /// <inheritdoc />
    public void Add(Job job)
    {
        ArgumentNullException.ThrowIfNull(job, nameof(job));
        ArgumentException.ThrowIfNullOrEmpty(job.Title, nameof(job.Title));
        
        _dbContext.Jobs.Add(job);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Job>> GetAllJobsAsync(CancellationToken ct = default) =>
        await _dbContext.Jobs.ToListAsync(ct);

    /// <inheritdoc />
    public async Task<Job?> GetByIdAsync(Guid jobId, CancellationToken ct = default) =>
        await _dbContext.Jobs.FindAsync(new object[] { jobId }, ct);

    /// <inheritdoc />
    public async Task<Job?> GetByTitleAsync(string title, CancellationToken ct = default) =>
        await _dbContext.Jobs.FirstOrDefaultAsync(j => j.Title == title, ct);

    /// <inheritdoc />
    public async Task<IEnumerable<Job>> GetJobsByClientIdAsync(Guid clientId, CancellationToken ct = default) =>
        await _dbContext.Jobs.Where(j => j.ClientId == clientId).ToListAsync(ct);

    /// <inheritdoc />
    public async Task<IEnumerable<Job>> GetUncompletedJobsAsync(CancellationToken ct = default) =>
        await _dbContext.Jobs
            .Where(m => m.Status != JobStatus.Completed && m.Status != JobStatus.Cancelled)
            .ToListAsync(ct);
}
