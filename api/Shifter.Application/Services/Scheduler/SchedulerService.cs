using Shifter.Application.DTOs.Scheduler;
using Shifter.Application.Interfaces.Repositories.Core;
using Shifter.Application.Interfaces.Repositories.Timesheets;
using Shifter.Application.Interfaces.Services.Locations;
using Shifter.Application.Interfaces.Services.Scheduler;
using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Timesheets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Application.Services.Scheduler;

public class SchedulerService(
    IWorkEventRepository workEventRepository,
    IAddressHandler addressHandler,
    IUnitOfWork unitOfWork) : ISchedulerService
{
    private readonly IWorkEventRepository _workEventRepository = workEventRepository;
    private readonly IAddressHandler _addressHandler = addressHandler;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <inheritdoc/>
    public async Task<ServiceResult<bool>> ScheduleWorkEventAsync(ScheduleWorkEventRequest request, CancellationToken ct = default)
    {
        if (request == null)
            return ServiceResult<bool>.Failure("Request cannot be null.");
        if (request.ScheduledStartTime >= request.ScheduledEndTime)        
            return ServiceResult<bool>.Failure("Scheduled start time must be before scheduled end time.");
        if (request.UserProfileId == Guid.Empty)
            return ServiceResult<bool>.Failure("UserProfileId cannot be empty.");

        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var address = request.Address != null ? await _addressHandler.GetOrCreateAddressAsync(request.Address, ct) : null;

            var workEvent = new WorkEvent
            {
                UserProfileId = request.UserProfileId,
                ScheduledStartTime = request.ScheduledStartTime,
                ScheduledEndTime = request.ScheduledEndTime,
                Title = request.Title,
                Description = request.Description,
                Notes = request.Notes,
                Address = address,
            };
            _workEventRepository.Add(workEvent);
            await _unitOfWork.CommitAsync(ct);
            return ServiceResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return ServiceResult<bool>.Failure($"Error when scheduling new work event. Error: {ex.Message}");
        }        
    }
}
