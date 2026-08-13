using Shifter.Application.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Application.DTOs.Scheduler;

public class ScheduleWorkEventRequest
{
    public Guid UserProfileId { get; set; }
    public DateTime ScheduledStartTime { get; set; }
    public DateTime ScheduledEndTime { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public AddressDto? Address { get; set; }
    public string Notes { get; set; } = string.Empty;
}
