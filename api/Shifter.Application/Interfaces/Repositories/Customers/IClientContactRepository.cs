using Shifter.Core.Entities.Customers;

namespace Shifter.Application.Interfaces.Repositories.Customers;

public interface IClientContactRepository
{
    /// <summary>
    /// Retrieves a ClientContact by its unique identifier asynchronously.
    /// </summary>
    /// <param name="clientContactId">The unique identifier of the ClientContact to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the ClientContact if found; otherwise, null.</returns>
    Task<ClientContact?> GetByIdAsync(Guid clientContactId);

    /// <summary>
    /// Adds a new ClientContact to the repository asynchronously.
    /// </summary>
    /// <param name="clientContact">The ClientContact to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(ClientContact clientContact);

    /// <summary>
    /// Retrieves all ClientContacts from the repository asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of ClientContacts.</returns>
    Task<IEnumerable<ClientContact>> GetAllAsync();
}
