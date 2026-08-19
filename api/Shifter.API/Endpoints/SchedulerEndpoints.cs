using Microsoft.AspNetCore.Mvc;
using Shifter.Application.DTOs.Scheduler;
using Shifter.Application.Interfaces.Services.Scheduler;

namespace Shifter.API.Endpoints;

public static class SchedulerEndpoints
{
    public static void MapSchedulerEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/scheduler");
        group.MapPost("/schedule-work-event", ScheduleWorkEventAsync);
    }

    private static async Task<IResult> ScheduleWorkEventAsync([FromBody] ScheduleWorkEventRequest request, ISchedulerService schedulerService)
    {
        var result = await schedulerService.ScheduleWorkEventAsync(request);
        if (!result.IsSuccess)
        {
            return Results.BadRequest(result.Error);
        }
        return Results.Ok("Work event scheduled successfully.");
    }
}
