using Microsoft.AspNetCore.Identity;
using Shifter.Core.Entities.HR;
using Shifter.Core.Entities.Tenant;

namespace Shifter.Core.Entities.Identity;

public class User : IdentityUser<Guid>
{
    public Role Role { get; set; } = Role.Employee;

    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }

    public EmployeeProfile? EmployeeProfile { get; set; }
}

