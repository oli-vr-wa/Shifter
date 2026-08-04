using Shifter.Core.Entities.Common;

namespace Shifter.Core.Entities.Location;

public class Address : BaseEntity, IMultiTenant
{
    public Guid CompanyId { get; set; } // Multi-tenant lock

    // Address Details
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
}
