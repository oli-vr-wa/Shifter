using Shifter.Core.Entities.Location;

namespace Shifter.Application.Interfaces.Repositories.Location;

public interface IAddressRepository
{
    /// <summary>
    /// Adds a new address to the repository.
    /// </summary>
    /// <param name="address">The address to add.</param>
    void Add(Address address);

    /// <summary>
    /// Retrieves an address by its PlaceId.
    /// </summary>
    /// <param name="placeId">The PlaceId of the address to retrieve.</param>
    /// <returns>The address with the specified PlaceId, or null if not found.</returns>
    Task<Address?> GetByPlaceIdAsync(string placeId, CancellationToken ct = default);

    /// <summary>
    /// Retrieves an address by its PlaceId and StreetNumber.
    /// </summary>
    /// <param name="placeId">The PlaceId of the address to retrieve.</param>
    /// <param name="streetNumber">The StreetNumber of the address to retrieve.</param>
    /// <returns>The address with the specified PlaceId and StreetNumber, or null if not found.</returns>
    Task<Address?> GetByPlaceIdAndStreetNumberAsync(string placeId, string streetNumber, CancellationToken ct = default);
}
