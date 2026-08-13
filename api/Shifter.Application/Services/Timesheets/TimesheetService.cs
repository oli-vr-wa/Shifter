using Shifter.Application.DTOs.Timesheet;
using Shifter.Application.Interfaces.Repositories.Core;
using Shifter.Application.Interfaces.Repositories.Timesheets;
using Shifter.Application.Interfaces.Services.Locations;
using Shifter.Application.Interfaces.Services.Timesheets;
using Shifter.Application.Interfaces.Tenant;
using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Timesheets;
using Shifter.Core.Entities.Timesheets.Enums;

namespace Shifter.Application.Services.Timesheets;

public class TimesheetService(
    IWorkEventHistoryLogHandler workEventHistoryLogHandler,
    IAddressHandler addressHandler,
    IWorkEventRepository workEventRepository, 
    ITenantService tenantService,
    IUnitOfWork unitOfWork) : ITimesheetService
{    
    private readonly IWorkEventHistoryLogHandler _workEventHistoryLogHandler = workEventHistoryLogHandler;    
    private readonly IAddressHandler _addressHandler = addressHandler;
    private readonly IWorkEventRepository _workEventRepository = workEventRepository;
    private readonly ITenantService _tenantService = tenantService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    /// <inheritdoc/>
    public async Task<ServiceResult<bool>> StartWorkEventTaskAsync(StartWorkEventTaskRequest request)
    {
        if (request == null)        
            return ServiceResult<bool>.Failure("Request cannot be null.");

        // Validate the request
        if (request.UserProfileId == Guid.Empty)
            return ServiceResult<bool>.Failure("UserProfileId cannot be empty.");

        // If a Work Event ID is provided, retrieve it
        var workEvent = request.WorkEventId.HasValue
            ? await _workEventRepository.GetByIdAsync(request.WorkEventId.Value)
            : null;

        using var transaction = await _unitOfWork.BeginTransactionAsync();

        // If an address is provided, add or update it and get the AddressId
        int? addressLocationId = request.Address != null ? await _addressHandler.AddOrUpdateAddressAsync(request.Address) : null;
        int? actionPerformedAtAddressId = request.ActionPerformedAtAddress != null ? await _addressHandler.AddOrUpdateAddressAsync(request.ActionPerformedAtAddress) : null;

        try
        {
            if (workEvent == null)
            {
                workEvent = new WorkEvent
                {
                    UserProfileId = request.UserProfileId,
                    Type = WorkEventType.JobTask,
                    Title = request.Title,
                    Description = request.Description,
                    CreatedByUserId = _tenantService.GetCurrentUserId(),
                    CreatedAt = DateTime.UtcNow,
                    AddressId = addressLocationId,
                };

                await _workEventRepository.AddAsync(workEvent);
                await _unitOfWork.CommitAsync();
                await _workEventHistoryLogHandler.AddWorkEventHistoryLogAsync(workEvent.Id, WorkEventStatus.Created, actionPerformedAtAddressId);
            }

            workEvent.ActualStartTime = request.ActualStartTime ?? DateTime.UtcNow;
            workEvent.Status = WorkEventStatus.Started;
            workEvent.LastUpdatedAt = DateTime.UtcNow;
            await _workEventHistoryLogHandler.AddWorkEventHistoryLogAsync(workEvent.Id, WorkEventStatus.Started, actionPerformedAtAddressId);
            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception($"An error occurred while starting the work event task: {ex.Message}", ex);
        }        

        return ServiceResult<bool>.Success(true);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<bool>> EndWorkEventTaskAsync(EndWorkEventTaskRequest request)
    {
        if (request == null)
            return ServiceResult<bool>.Failure("Request cannot be null.");
        if (request.UserProfileId == Guid.Empty)
            return ServiceResult<bool>.Failure("UserProfileId cannot be empty.");
        if (request.WorkEventId == Guid.Empty)
            return ServiceResult<bool>.Failure("WorkEventId cannot be empty.");

        // Retrieve the work event
        var workEvent = await _workEventRepository.GetByIdAsync(request.WorkEventId);
        if (workEvent == null)
            return ServiceResult<bool>.Failure("Work event not found.");

        using var transaction = await _unitOfWork.BeginTransactionAsync();

        int? addressLocationId = request.ActionPerformedAtAddress != null ? await _addressHandler.AddOrUpdateAddressAsync(request.ActionPerformedAtAddress) : null;

        try
        {
            workEvent.ActualEndTime = request.ActualEndTime;
            workEvent.Status = WorkEventStatus.Completed;
            workEvent.LastUpdatedAt = DateTime.UtcNow;
            await _workEventHistoryLogHandler.AddWorkEventHistoryLogAsync(workEvent.Id, WorkEventStatus.Completed, addressLocationId);
            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception($"An error occurred while ending the work event task: {ex.Message}", ex);
        }        

        return ServiceResult<bool>.Success(true);
    }
}
