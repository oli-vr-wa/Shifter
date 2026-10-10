using Shifter.Application.DTOs.Location;
using Shifter.Core.Entities.Operations;

namespace Shifter.Application.DTOs.Operations;

public record JobDto(
    Guid Id,
    Guid ClientId,
    string Title,
    string Description,
    string PurchaseOrderNumber,
    JobPriority Priority,
    JobStatus Status,
    DateTime? ReceivedDate,
    DateTime? TargetStartDate,
    DateTime? Deadline,
    DateTime? CompletionDate,
    AddressDto? SiteAddress
);
