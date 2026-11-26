using Microsoft.EntityFrameworkCore;
using Shifter.Application.DTOs.Common;
using Shifter.Application.DTOs.Operations;
using Shifter.Application.Interfaces.Repositories.Operations;
using Shifter.Application.Interfaces.Services.Operations;
using Shifter.Application.Mappings.Operations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Application.Services.Operations;

public class JobQueryService(IJobRepository jobRepository) : IJobQueryService
{
    private readonly IJobRepository _jobRepository = jobRepository;

    /// <inheritdoc />
    public async Task<JobDto?> GetByIdAsync(Guid jobId, CancellationToken ct = default)
    {
        var job = await _jobRepository.GetByIdAsync(jobId, ct);

        if (job is null)
        {
            return null;
        }

        return job.ToDto();
    }

    /// <inheritdoc />
    public async Task<PageResult<JobListItemDto>> GetAllAsync(JobQueryFilter filter, CancellationToken ct = default)
    {
        var safePageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber;
        var safePageSize = filter.PageSize <= 0 ? 20 : filter.PageSize;

        var query = _jobRepository.GetQueryable();

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            query = query.Where(j => (j.Title != null && j.Title.Contains(filter.SearchTerm)) ||
                                     (j.Description != null && j.Description.Contains(filter.SearchTerm)) ||
                                     (j.PurchaseOrderNumber != null && j.PurchaseOrderNumber.Contains(filter.SearchTerm)));
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(j => j.Status == filter.Status.Value);
        }

        if (filter.Priority.HasValue)
        {
            query = query.Where(j => j.Priority == filter.Priority.Value);
        }

        if (filter.ClientId.HasValue)
        {
            query = query.Where(j => j.ClientId == filter.ClientId.Value);
        }

        var totalCount = await query.CountAsync(ct);

        // Apply sorting
        query = filter.SortColumn?.ToLower() switch
        {
            "title" => filter.SortDescending ? query.OrderByDescending(j => j.Title) : query.OrderBy(j => j.Title),
            "receiveddate" => filter.SortDescending ? query.OrderByDescending(j => j.ReceivedDate) : query.OrderBy(j => j.ReceivedDate),
            "deadline" => filter.SortDescending ? query.OrderByDescending(j => j.Deadline) : query.OrderBy(j => j.Deadline),
            _ => query.OrderBy(j => j.CreatedAt) // Default sorting
        };

        var items = await query
            .Skip((safePageNumber - 1) * safePageSize)
            .Take(safePageSize)
            .Select(JobMappingExtensions.ProjectToListItem)
            .ToListAsync(ct);

        return new PageResult<JobListItemDto>(items, totalCount, safePageNumber, safePageSize);
    }

    /// <inheritdoc />
    public Task<PageResult<JobListItemDto>> GetByClientIdAsync(Guid clientId, JobQueryFilter filter, CancellationToken ct = default)
    {
        var filterWithClient = filter with { ClientId = clientId };

        return GetAllAsync(filterWithClient, ct);
    }    
}
