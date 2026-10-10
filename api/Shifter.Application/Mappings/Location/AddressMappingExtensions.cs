using Shifter.Application.DTOs.Location;
using Shifter.Core.Entities.Location;

namespace Shifter.Application.Mappings.Location;

public static class AddressMappingExtensions
{
    /// <summary>
    /// Maps an Address entity to an AddressDto.
    /// </summary>
    /// <param name="address">The Address entity to map.</param>
    /// <returns>An AddressDto representing the mapped Address entity.</returns>
    public static AddressDto ToDto(this Address address) => new(
        address.PlaceId,
        address.DisplayName,
        address.StreetNumber,
        address.StreetName,
        address.Suburb,
        address.State,
        address.PostCode,
        address.Country,
        address.Latitude,
        address.Longitude
    );
}
