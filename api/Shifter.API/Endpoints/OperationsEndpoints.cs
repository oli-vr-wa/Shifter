using Microsoft.AspNetCore.Mvc;
using Shifter.Application.DTOs.Operations;
using Shifter.Application.Interfaces.Services.Operations;

namespace Shifter.API.Endpoints;

public static class OperationsEndpoints
{
    public static void MapOperationsEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/operations");
        group.MapGet("/jobs", GetAllJobsAsync);
        group.MapGet("/jobs/{jobId:guid}", GetJobByIdAsync);
        group.MapGet("/clients/{clientId:guid}/jobs", GetJobsByClientIdAsync);
        group.MapPost("/jobs", CreateJobAsync);
        group.MapPatch("/jobs/{jobId:guid}", UpdateJobAsync);
    }

    private static async Task<IResult> GetAllJobsAsync([AsParameters] JobQueryFilter filter, IJobQueryService jobQueryService, CancellationToken ct)
    {
        var jobs = await jobQueryService.GetAllAsync(filter, ct);
        return Results.Ok(jobs);
    }

    private static async Task<IResult> GetJobByIdAsync(Guid jobId, IJobQueryService jobQueryService, CancellationToken ct)
    {
        var job = await jobQueryService.GetByIdAsync(jobId, ct);
        if (job == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(job);
    }

    private static async Task<IResult> GetJobsByClientIdAsync(Guid clientId, [AsParameters] JobQueryFilter filter, IJobQueryService jobQueryService, CancellationToken ct)
    {
        var jobs = await jobQueryService.GetByClientIdAsync(clientId, filter, ct);
        return Results.Ok(jobs);
    }

    private static async Task<IResult> CreateJobAsync([FromBody] JobDto jobDto, IJobCommandService jobCommandService, CancellationToken ct)
    {
        var result = await jobCommandService.CreateAsync(jobDto, ct);

        return Results.Created($"/api/operations/jobs/{result.Value}", result);
    }

    private static async Task<IResult> UpdateJobAsync(Guid jobId, [FromBody] JobDto jobUpdateDto, IJobCommandService jobCommandService, CancellationToken ct)
    {
        var job = await jobCommandService.UpdateAsync(jobId, jobUpdateDto, ct);
        if (job == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(job);
    }
}
