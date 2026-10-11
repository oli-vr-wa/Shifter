using Shifter.Application.DTOs.Location;
using Shifter.Core.Entities.Location;

namespace Shifter.Application.Interfaces.Services.Locations;

public interface IAddressHandler
{
    /// <summary>
    /// Retrieves an existing address based on the provided PlaceId or creates a new address if it doesn't exist.
    /// </summary>
    /// <param name="address">The address to get or create.</param>
    /// <param name="ct">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved or newly created address.</returns>
    Task<Address> GetOrCreateAddressAsync(AddressDto address, CancellationToken ct = default);
}
