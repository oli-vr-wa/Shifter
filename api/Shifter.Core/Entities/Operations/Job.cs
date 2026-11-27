using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Customers;
using Shifter.Core.Entities.Location;
using Shifter.Core.Entities.Timesheets;

namespace Shifter.Core.Entities.Operations;

public class Job : BaseEntity, IMultiTenant
{
    public Guid CompanyId { get; set; }

    public Guid? ClientId { get; set; }
    public Client? Client { get; set; }

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

    public static Job Create(Guid? clientId, string title, string? description, string? purchaseOrderNumber, JobPriority priority, JobStatus status, DateTime? receivedDate, DateTime? targetStartDate, DateTime? deadline, DateTime? completionDate)
    {
        var job = new Job
        {
            ClientId = clientId,
            Title = title,
            Description = description,
            PurchaseOrderNumber = purchaseOrderNumber,
            Priority = priority,
            Status = status,
            ReceivedDate = receivedDate,
            TargetStartDate = targetStartDate,
            Deadline = deadline,
            CompletionDate = completionDate,
        };

        job.Validate();
        return job;
    }

    public void Update(string title, string? description, string? purchaseOrderNumber, JobPriority priority, JobStatus status, DateTime? receivedDate, DateTime? targetStartDate, DateTime? deadline, DateTime? completionDate)
    {
        Title = title;
        Description = description;
        PurchaseOrderNumber = purchaseOrderNumber;
        Priority = priority;
        Status = status;
        ReceivedDate = receivedDate;
        TargetStartDate = targetStartDate;
        Deadline = deadline;
        CompletionDate = completionDate;

        Validate();
    }

    private void Validate()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Title, nameof(Title));        

        if (ClientId == Guid.Empty)
        {
            throw new ArgumentException("Client ID cannot be empty.", nameof(ClientId));
        }
        if (ReceivedDate.HasValue && TargetStartDate.HasValue && ReceivedDate > TargetStartDate)
        {
            throw new ArgumentException("Received date cannot be later than target start date.");
        }
        if (TargetStartDate.HasValue && Deadline.HasValue && TargetStartDate > Deadline)
        {
            throw new ArgumentException("Target start date cannot be later than deadline.");
        }
        if (Deadline.HasValue && CompletionDate.HasValue && Deadline > CompletionDate)
        {
            throw new ArgumentException("Deadline cannot be later than completion date.");
        }
    }
}
