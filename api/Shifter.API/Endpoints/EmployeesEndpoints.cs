using Shifter.Application.DTOs.HumanResources;
using Shifter.Application.Interfaces.Services.HumanResources;

namespace Shifter.API.Endpoints;

public static class EmployeesEndpoints
{
    public static void MapEmployeesEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/employees");

        // Employees lists 
        group.MapGet("/", GetAllEmployeesAsync);
        group.MapGet("/current", GetAllCurrentEmployeesAsync);
        group.MapGet("/terminated", GetAllTerminatedEmployeesAsync);
        // Create, Get, Update
        group.MapPost("/", CreateEmployeeAsync); 
        group.MapGet("/{employeeId:guid}", GetEmployeeByIdAsync);
        group.MapPut("/{employeeId:guid}", UpdateEmployeeAsync);
    }

    private static async Task<IResult> GetAllEmployeesAsync(IEmployeeQueryService employeeQueryService, CancellationToken ct)
    {
        var employees = await employeeQueryService.GetAllAsync(ct);
        return Results.Ok(employees);
    }

    private static async Task<IResult> GetAllCurrentEmployeesAsync(IEmployeeQueryService employeeQueryService, CancellationToken ct)
    {
        var employees = await employeeQueryService.GetAllCurrentAsync(ct);
        return Results.Ok(employees);
    }

    private static async Task<IResult> GetAllTerminatedEmployeesAsync(IEmployeeQueryService employeeQueryService, CancellationToken ct)
    {
        var employees = await employeeQueryService.GetAllTerminatedAsync(ct);
        return Results.Ok(employees);
    }

    private static async Task<IResult> CreateEmployeeAsync(IEmployeeOnboardingService employeeOnboardingService, OnboardEmployeeRequest request, CancellationToken ct)
    {
        var employeeId = await employeeOnboardingService.OnboardAsync(request, ct);
        return Results.Created($"/api/employees/{employeeId}", employeeId);
    }

    private static async Task<IResult> GetEmployeeByIdAsync(Guid employeeId, IEmployeeQueryService employeeQueryService, CancellationToken ct)
    {
        var employee = await employeeQueryService.GetByIdAsync(employeeId, ct);
        if (employee == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(employee);
    }

    private static async Task<IResult> UpdateEmployeeAsync(Guid employeeId, OnboardEmployeeRequest request, IEmployeeOnboardingService employeeOnboardingService, CancellationToken ct)
    {
        var result = await employeeOnboardingService.UpdateAsync(employeeId, request, ct);

        return result.IsSuccess
            ? Results.Ok()
            : Results.BadRequest(result.Error);
    }
}
