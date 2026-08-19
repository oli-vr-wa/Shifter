using Shifter.Application.Interfaces.Services.HumanResources;

namespace Shifter.API.Endpoints;

public static class EmployeesEndpoints
{
    public static void MapEmployeesEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/employees");

        group.MapGet("/", GetAllEmployeesAsync);
        group.MapGet("/current", GetAllCurrentEmployeesAsync);
        group.MapGet("/terminated", GetAllTerminatedEmployeesAsync);
    }

    private static async Task<IResult> GetAllEmployeesAsync(IEmployeeQueryService employeeQueryService)
    {
        var employees = await employeeQueryService.GetAllAsync();
        return Results.Ok(employees);
    }

    private static async Task<IResult> GetAllCurrentEmployeesAsync(IEmployeeQueryService employeeQueryService)
    {
        var employees = await employeeQueryService.GetAllCurrentAsync();
        return Results.Ok(employees);
    }

    private static async Task<IResult> GetAllTerminatedEmployeesAsync(IEmployeeQueryService employeeQueryService)
    {
        var employees = await employeeQueryService.GetAllTerminatedAsync();
        return Results.Ok(employees);
    }
}
