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
        var query = _jobRepository.GetQueryable();

        if (filter.SearchTerm is { } term)
        {
            query = query.Where(j => j.Title.ToLower().Contains(term) ||
                                     (j.Description != null && j.Description.ToLower().Contains(term)) ||
                                     (j.PurchaseOrderNumber != null && j.PurchaseOrderNumber.ToLower().Contains(term)));
        }

        if (filter.Status.HasValue) query = query.Where(j => j.Status == filter.Status.Value);       
        if (filter.Priority.HasValue) query = query.Where(j => j.Priority == filter.Priority.Value);
        if (filter.ClientId.HasValue) query = query.Where(j => j.ClientId == filter.ClientId.Value);

        var totalCount = await query.CountAsync(ct);

        // Apply sorting
        query = filter.SortColumn switch
        {
            "title" => filter.SortDescending 
                ? query.OrderByDescending(j => j.Title).ThenByDescending(j => j.Id) 
                : query.OrderBy(j => j.Title).ThenBy(j => j.Id),
            "receiveddate" => filter.SortDescending 
                ? query.OrderByDescending(j => j.ReceivedDate).ThenByDescending(j => j.Id)
                : query.OrderBy(j => j.ReceivedDate).ThenBy(j => j.Id),
            "deadline" => filter.SortDescending 
                ? query.OrderByDescending(j => j.Deadline).ThenByDescending(j => j.Id) 
                : query.OrderBy(j => j.Deadline).ThenBy(j => j.Id),
            _ => filter.SortDescending 
                ? query.OrderByDescending(j => j.CreatedAt).ThenByDescending(j => j.Id) 
                : query.OrderBy(j => j.CreatedAt).ThenBy(j => j.Id) // Default sorting
        };

        var items = await query
            .Skip(filter.Skip)
            .Take(filter.PageSize)
            .Select(JobMappingExtensions.ProjectToListItem)
            .ToListAsync(ct);

        return new PageResult<JobListItemDto>(items, totalCount, filter.PageNumber, filter.PageSize);
    }

    /// <inheritdoc />
    public Task<PageResult<JobListItemDto>> GetByClientIdAsync(Guid clientId, JobQueryFilter filter, CancellationToken ct = default)
    {
        var filterWithClient = filter with { ClientId = clientId };

        return GetAllAsync(filterWithClient, ct);
    }    
}
