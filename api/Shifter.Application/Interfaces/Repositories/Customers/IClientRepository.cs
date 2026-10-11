using Shifter.Core.Entities.Customers;

namespace Shifter.Application.Interfaces.Repositories.Customers;

public interface IClientRepository
{
    /// <summary>
    /// Retrieves a Client by its unique identifier asynchronously.
    /// </summary>
    /// <param name="clientId">The unique identifier of the Client to retrieve.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Client if found; otherwise, null.</returns>
    Task<Client?> GetByIdAsync(Guid clientId, CancellationToken ct);

    /// <summary>
    /// Adds a new Client to the repository asynchronously.
    /// </summary>
    /// <param name="client">The Client to add.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    void Add(Client client);

    /// <summary>
    /// Retrieves all Clients from the repository asynchronously.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of Clients.</returns>
    Task<IEnumerable<Client>> GetAllAsync(CancellationToken ct);

    /// <summary>
    /// Retrieves an IQueryable of Clients for further querying.
    /// </summary>
    /// <returns>An IQueryable of Clients.</returns>
    IQueryable<Client> GetQueryable();
}
