using Shifter.Application.DTOs.Customers;

namespace Shifter.Application.Interfaces.Services.Customers;

public interface IClientCommandService
{
    /// <summary>
    /// Creates a new client and returns the unique identifier (GUID) of the newly created client.
    /// </summary>
    /// <param name="clientDto">The data transfer object containing the details of the client to be created.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The unique identifier (GUID) of the newly created client.</returns>
    Task<Guid> CreateAsync(ClientDto clientDto, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing client with the specified unique identifier (GUID) using the provided data transfer object (ClientDto).
    /// </summary>
    /// <param name="guid">The unique identifier (GUID) of the client to be updated.</param>
    /// <param name="clientDto">The data transfer object containing the updated details of the client.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateAsync(Guid guid, ClientDto clientDto, CancellationToken ct = default);
}
