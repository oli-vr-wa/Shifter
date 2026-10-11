using Microsoft.AspNetCore.Mvc;
using Shifter.Application.DTOs.Timesheet;
using Shifter.Application.Interfaces.Services.Timesheets;

namespace Shifter.API.Endpoints;

public static class TimesheetServiceEndpoints
{
    public static void MapTimesheetServiceEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/timesheet-service");
        group.MapPost("/start-work-event-task", StartWorkEventTaskAsync);
        group.MapPost("/end-work-event-task", EndWorkEventTaskAsync);
    }
    private static async Task<IResult> StartWorkEventTaskAsync([FromBody] StartWorkEventTaskRequest request, ITimesheetService timesheetService, CancellationToken ct)
    {
        var result = await timesheetService.StartWorkEventTaskAsync(request, ct);
        if (!result.IsSuccess)
        {
            return Results.BadRequest(result.Error);
        }
        return Results.Ok("Work event task started successfully.");
    }

    private static async Task<IResult> EndWorkEventTaskAsync([FromBody] EndWorkEventTaskRequest request, ITimesheetService timesheetService, CancellationToken ct)
    {
        var result = await timesheetService.EndWorkEventTaskAsync(request, ct);
        if (!result.IsSuccess)
        {
            return Results.BadRequest(result.Error);
        }
        return Results.Ok("Work event task ended successfully.");
    }
}
