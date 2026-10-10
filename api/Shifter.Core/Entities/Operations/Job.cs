using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Customers;
using Shifter.Core.Entities.Location;
using Shifter.Core.Entities.Timesheets;

namespace Shifter.Core.Entities.Operations;

public class Job : BaseEntity, IMultiTenant
{
    public Guid CompanyId { get; set; }

    public Guid ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? PurchaseOrderNumber { get; set; }

    public JobPriority Priority { get; set; } = JobPriority.Normal;
    public JobStatus Status { get; set; } = JobStatus.Pending;

    public DateTime? ReceivedDate { get; set; }
    public DateTime? TargetStartDate { get; set; }
    public DateTime? Deadline { get; set; }
    public DateTime? CompletionDate { get; set; }

    public Address? SiteAddress { get; set; }

    public ICollection<WorkEvent> WorkEvents { get; set; } = new List<WorkEvent>();
}
