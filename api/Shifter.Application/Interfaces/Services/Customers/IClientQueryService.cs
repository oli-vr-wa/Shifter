using Shifter.Application.DTOs.Common;
using Shifter.Application.DTOs.Customers;

namespace Shifter.Application.Interfaces.Services.Customers;

public interface IClientQueryService
{
    /// <summary>
    /// Gets a client by its unique identifier.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The client DTO if found; otherwise, null.</returns>
    Task<ClientDto?> GetClientByIdAsync(Guid clientId, CancellationToken ct);

    /// <summary>
    /// Gets a paginated list of clients based on the specified filter.
    /// </summary>
    /// <param name="filter">The filter criteria for querying clients.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A paginated list of client list item DTOs.</returns>
    Task<PageResult<ClientListItemDto>> GetClientsAsync(ClientQueryFilter filter, CancellationToken ct);    
}
