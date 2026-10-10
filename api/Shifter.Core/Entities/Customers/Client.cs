using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Location;
using Shifter.Core.Entities.Operations;

namespace Shifter.Core.Entities.Customers;

public class Client : BaseEntity, IMultiTenant
{
    public Guid CompanyId { get; set; }
    public required ClientType Type { get; set; }

    // Business Information
    public string? CompanyName { get; set; }
    public string? Abn { get; set; }

    // Individual Information
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    // Contact Information
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }

    public string DisplayName => Type == ClientType.Individual ? $"{FirstName} {LastName}" : CompanyName ?? string.Empty;

    public ICollection<ClientContact> Contacts { get; set; } = new List<ClientContact>();
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}
