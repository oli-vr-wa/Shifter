using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.HR;

namespace Shifter.Core.Entities.Timesheets
{
    public class WorkEvent : BaseEntity
    {
        public Guid EmployeeProfileId { get; set; }
        public EmployeeProfile? EmployeeProfile { get; set; }
        public Guid CompanyId { get; set; }

        public WorkEventType Type { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public Guid? CustomerId { get; set; }
        public string? JobNumber { get; set; }

        public string? Notes { get; set; }
        public WorkEventStatus Status { get; set; }
    }
}
