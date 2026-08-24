using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Identity;

namespace Shifter.Core.Entities.Tenant;

public class UserProfile : BaseEntity, IMultiTenant
{
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
            
    public Guid UserId { get; set; }
    public User? User { get; set; }

    // Multi-tenant lock
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
}
