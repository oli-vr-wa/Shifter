using Shifter.Application.DTOs.Location;

namespace Shifter.Application.DTOs.Timesheet;

public class EndWorkEventTaskRequest
{
    public Guid UserProfileId { get; set; }
    public Guid WorkEventId { get; set; }
    public DateTime ActualEndTime { get; set; }
    public AddressDto? Address { get; set; }
    public string? Notes { get; set; }
}
