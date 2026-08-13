using Shifter.Application.DTOs.Location;
using Shifter.Application.Interfaces.Repositories.Timesheets;
using Shifter.Application.Interfaces.Services.Locations;
using Shifter.Application.Interfaces.Services.Timesheets;
using Shifter.Application.Interfaces.Tenant;
using Shifter.Core.Entities.Timesheets;
using Shifter.Core.Entities.Timesheets.Enums;

namespace Shifter.Application.Services.Timesheets.Handlers;

public class WorkEventHistoryLogHandler(
    IWorkEventHistoryRepository workEventHistoryRepository,
    ITenantService tenantService) : IWorkEventHistoryLogHandler
{
    private readonly IWorkEventHistoryRepository _workEventHistoryRepository = workEventHistoryRepository;
    private readonly ITenantService _tenantService = tenantService;

    /// <inheritdoc />
    public async Task AddWorkEventHistoryLogAsync(Guid workEventId, WorkEventStatus status, int? addressLocationId)
    {
        if (workEventId == Guid.Empty)        
            throw new ArgumentException("Work event ID cannot be empty.", nameof(workEventId));

        string description = GetDescriptionForStatus(status);;

        var logEntry = InitializeWorkEventHistoryLog(status, description, addressLocationId);
        logEntry.WorkEventId = workEventId;

        await _workEventHistoryRepository.AddAsync(logEntry);
    }

    /// <inheritdoc />
    public async Task AddBreakEventHistoryLogAsync(Guid breakEventId, WorkEventStatus status, int? addressLocationId)
    {
        if (breakEventId == Guid.Empty)
            throw new ArgumentException("Break event ID cannot be empty.", nameof(breakEventId));

        string description = GetDescriptionForStatus(status);

        var logEntry = InitializeWorkEventHistoryLog(status, description, addressLocationId);
        logEntry.BreakEventId = breakEventId;

        await _workEventHistoryRepository.AddAsync(logEntry);
    }

    /// <summary>
    /// Gets a description for the given work event status.
    /// </summary>
    /// <param name="status">The work event status.</param>
    /// <returns>A description for the given work event status.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the work event status is unhandled.</exception>
    private string GetDescriptionForStatus(WorkEventStatus status)
    {
        return status switch
        {
            WorkEventStatus.Scheduled => "Work event scheduled.",
            WorkEventStatus.Created => "Work event created.",
            WorkEventStatus.Started => "Work event clocked in.",
            WorkEventStatus.Completed => "Work event clocked out.",
            WorkEventStatus.Approved => "Work event approved.",
            WorkEventStatus.Rejected => "Work event rejected.",
            WorkEventStatus.Locked => "Work event locked for payment.",
            _ => throw new ArgumentOutOfRangeException(nameof(status), $"Unhandled work event status: {status}")
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkEventHistory"/> class with the provided status and description.
    /// </summary>
    /// <param name="status">The work event status.</param>
    /// <param name="description">The description of the work event history.</param>
    /// <param name="addressId">The ID of the address associated with the work event history.</param>
    /// <returns>A new instance of the <see cref="WorkEventHistory"/> class.</returns>
    private WorkEventHistory InitializeWorkEventHistoryLog(WorkEventStatus status, string description, int? addressId)
    {
        return new WorkEventHistory
        {
            Status = status,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            ActionPerformedAt = DateTime.UtcNow,
            ActionPerformedByUserId = _tenantService.GetCurrentUserId(),
            ActionPerformedAtAddressId = addressId
        };
    }
}
