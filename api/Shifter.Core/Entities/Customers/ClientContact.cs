using Shifter.Core.Entities.Common;

namespace Shifter.Core.Entities.Customers;

public class ClientContact : BaseEntity, IMultiTenant
{
    public Guid CompanyId { get; set; }

    public Guid ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? JobTitle { get; set; }

    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    public bool IsPrimary { get; set; } = false;
}
