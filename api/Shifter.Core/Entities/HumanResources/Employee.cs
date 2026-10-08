using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Identity;
using Shifter.Core.Entities.Tenant;

namespace Shifter.Core.Entities.HumanResources;

public class Employee : BaseEntity, IMultiTenant
{
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public string? JobPosition { get; set; }
    public DateTime EmploymentStartDate { get; set; }
    public DateTime? EmploymentTerminationDate { get; set; }
}
