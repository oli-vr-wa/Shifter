using Microsoft.AspNetCore.Identity;
using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Tenant;

namespace Shifter.Core.Entities.Identity;

public class User : IdentityUser<Guid>, IMultiTenant
{
    public Role Role { get; set; } = Role.Employee;

    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }

    public UserProfile? EmployeeProfile { get; set; }
}

