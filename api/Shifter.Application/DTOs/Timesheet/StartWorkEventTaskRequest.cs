using Shifter.Application.DTOs.Location;

namespace Shifter.Application.DTOs.Timesheet;

public class StartWorkEventTaskRequest
{
    public Guid UserProfileId { get; set; }
    // When the task has not been scheduled yet and the employee creates the task, there won't be a work event id, so it will be null.
    public Guid? WorkEventId { get; set; }

    public DateTime? ActualStartTime { get; set; }    

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public AddressDto? Address { get; set; }

    public string? Notes { get; set; }
}
