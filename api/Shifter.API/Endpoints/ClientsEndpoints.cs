using Microsoft.AspNetCore.Mvc;
using Shifter.Application.DTOs.Customers;
using Shifter.Application.Interfaces.Services.Customers;

namespace Shifter.API.Endpoints;

public static class ClientsEndpoints
{
    public static void MapClientsEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/clients");
        // Clients lists 
        group.MapGet("/", GetAllClientsAsync);
        group.MapGet("/{clientId:guid}", GetClientByIdAsync);
        group.MapPost("/", CreateClientAsync);
        group.MapPut("/{clientId:guid}", UpdateClientAsync);
    }

    private static async Task<IResult> GetAllClientsAsync([FromBody] ClientQueryFilter filter, IClientQueryService clientQueryService, CancellationToken ct)
    {
        var clients = await clientQueryService.GetClientsAsync(filter, ct);
        return Results.Ok(clients);
    }
    
    private static async Task<IResult> GetClientByIdAsync(Guid clientId, IClientQueryService clientQueryService, CancellationToken ct)
    {
        var client = await clientQueryService.GetByIdAsync(clientId, ct);
        if (client == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(client);
    }

    private static async Task<IResult> CreateClientAsync([FromBody] ClientDto request, IClientCommandService clientCommandService, CancellationToken ct)
    {
        var result = await clientCommandService.CreateAsync(request, ct);
        if (!result.IsSuccess)
        {
            return Results.BadRequest(result.Error);
        }
        return Results.Ok(result.Value);
    }

    private static async Task<IResult> UpdateClientAsync(Guid clientId, [FromBody] ClientDto request, IClientCommandService clientCommandService, CancellationToken ct)
    {
        var result = await clientCommandService.UpdateAsync(clientId, request, ct);
        if (!result.IsSuccess)
        {
            return Results.BadRequest(result.Error);
        }
        return Results.Ok(result.Value);
    }
}
