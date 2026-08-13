using Shifter.Application.DTOs.Location;

namespace Shifter.Application.Interfaces.Services.Locations;

public interface IAddressHandler
{
    /// <summary>
    /// Adds or updates an address in the system. 
    /// If the address already exists (based on PlaceId), it will be updated; otherwise, a new address will be added.
    /// </summary>
    /// <param name="address">The address to add or update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the added or updated address.</returns>
    Task<int> AddOrUpdateAddressAsync(AddressDto address);
}
