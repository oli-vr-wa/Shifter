using Shifter.Core.Entities.Common;

namespace Shifter.Core.Entities.Location;

public class Address : IMultiTenant
{
    public int Id { get; set; } 

    public Guid CompanyId { get; set; } // Multi-tenant lock

    // Address Details
    public string PlaceId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string StreetNumber { get; set; } = string.Empty;
    public string StreetName { get; set; } = string.Empty;
    public string Suburb { get; set; } = string.Empty;
    public string PostCode { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    // Geopositioning for maps/geofencing
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public string ShortAddress => $"{StreetNumber} {StreetName}, {Suburb}, {State} {PostCode}";
    public string FullAddress => $"{ShortAddress}, {Country}";

    /// <summary>
    /// Creates a new Address instance with the provided details.
    /// </summary>
    /// <param name="placeId">The unique identifier for the place.</param>
    /// <param name="displayName">The display name of the address.</param>
    /// <param name="streetNumber">The street number of the address.</param>
    /// <param name="streetName">The street name of the address.</param>
    /// <param name="suburb">The suburb of the address.</param>
    /// <param name="postCode">The postal code of the address.</param>
    /// <param name="state">The state of the address.</param>
    /// <param name="country">The country of the address.</param>
    /// <param name="latitude">The latitude of the address.</param>
    /// <param name="longitude">The longitude of the address.</param>
    /// <returns>A new instance of the Address class.</returns>
    public static Address Create(string placeId, string displayName, string streetNumber, string streetName, string suburb, string postCode, string state, string country, double? latitude = null, double? longitude = null)
    {        
        var address = new Address
        {
            PlaceId = placeId,
            DisplayName = displayName,
            StreetNumber = streetNumber,
            StreetName = streetName,
            Suburb = suburb,
            PostCode = postCode,
            State = state,
            Country = country,
            Latitude = latitude,
            Longitude = longitude
        };
        address.Validate();
        return address;
    }

    /// <summary>
    /// Checks if the current address is equal to another address based on PlaceId and StreetNumber.
    /// </summary>
    /// <param name="placeId">The unique identifier for the place.</param>
    /// <param name="streetNumber">The street number of the address.</param>
    /// <returns>True if the current address is equal to the specified address; otherwise, false.</returns>
    public bool IsEqualTo(string placeId, string streetNumber)
    {
        return PlaceId == placeId && StreetNumber == streetNumber;
    }

    /// <summary>
    /// Validates the address properties to ensure they are not null or whitespace.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when any of the address properties are null or whitespace.</exception>
    private void Validate()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(PlaceId, nameof(PlaceId));
        ArgumentException.ThrowIfNullOrWhiteSpace(DisplayName, nameof(DisplayName));
        ArgumentException.ThrowIfNullOrWhiteSpace(StreetNumber, nameof(StreetNumber));
        ArgumentException.ThrowIfNullOrWhiteSpace(StreetName, nameof(StreetName));
        ArgumentException.ThrowIfNullOrWhiteSpace(Suburb, nameof(Suburb));
        ArgumentException.ThrowIfNullOrWhiteSpace(PostCode, nameof(PostCode));
        ArgumentException.ThrowIfNullOrWhiteSpace(State, nameof(State));
        ArgumentException.ThrowIfNullOrWhiteSpace(Country, nameof(Country));
    }
}
