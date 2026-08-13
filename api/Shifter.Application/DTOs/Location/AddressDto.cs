
namespace Shifter.Application.DTOs.Location;

public record AddressDto
(
    string PlaceId,
    string DisplayName,
    string StreetNumber,
    string StreetName,
    string Suburb,
    string PostCode,
    string State,
    string Country,
    double? Latitude,
    double? Longitude
);
