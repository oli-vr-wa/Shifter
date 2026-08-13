using Shifter.Core.Entities.Location;

namespace Shifter.Application.Interfaces.Repositories.Location;

public interface IAddressRepository
{
    /// <summary>
    /// Adds a new address to the repository.
    /// </summary>
    /// <param name="address">The address to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Add(Address address);

    /// <summary>
    /// Retrieves an address by its PlaceId.
    /// </summary>
    /// <param name="placeId">The PlaceId of the address to retrieve.</param>
    /// <returns>The address with the specified PlaceId, or null if not found.</returns>
    Address? GetByPlaceId(string placeId);
}
