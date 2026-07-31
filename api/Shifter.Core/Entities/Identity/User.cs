using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.HR;
using Shifter.Core.Entities.Tenant;

namespace Shifter.Core.Entities.Identity
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public Role Role { get; set; } = Role.Employee;

        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }

        public EmployeeProfile? EmployeeProfile { get; set; }
    }
}
