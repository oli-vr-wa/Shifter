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

    private static async Task<IResult> CreateEmployeeAsync(IEmployeeOnboardingService employeeOnboardingService, OnboardEmployeeRequest request)
    {
        var employeeId = await employeeOnboardingService.OnboardAsync(request);
        return Results.Created($"/api/employees/{employeeId}", employeeId);
    }

    private static async Task<IResult> GetEmployeeByIdAsync(Guid employeeId, IEmployeeQueryService employeeQueryService)
    {
        var employee = await employeeQueryService.GetByIdAsync(employeeId);
        if (employee == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(employee);
    }

    private static async Task<IResult> UpdateEmployeeAsync(Guid employeeId, OnboardEmployeeRequest request, IEmployeeOnboardingService employeeOnboardingService)
    {
        var result = await employeeOnboardingService.UpdateAsync(employeeId, request);

        return result.IsSuccess
            ? Results.Ok()
            : Results.BadRequest(result.Error);
    }
}
