using Shifter.Core.Entities.Operations;

namespace Shifter.Application.DTOs.Operations;

public record JobListItemDto(
    Guid Id,
    string Title,
    string? Description,
    string? PurchaseOrderNumber,
    JobPriority Priority,
    JobStatus Status,
    DateTime ReceivedDate,
    DateTime? TargetStartDate,
    DateTime? Deadline,
    DateTime? CompletionDate,
    string? SiteAddress
);
