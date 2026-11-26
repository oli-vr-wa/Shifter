using Shifter.Application.DTOs.Operations;
using Shifter.Application.Interfaces.Repositories.Core;
using Shifter.Application.Interfaces.Repositories.Operations;
using Shifter.Application.Interfaces.Services.Locations;
using Shifter.Application.Interfaces.Services.Operations;
using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Operations;

namespace Shifter.Application.Services.Operations;

public class JobCommandService(IJobRepository jobRepository, IAddressHandler addressHandler, IUnitOfWork unitOfWork) : IJobCommandService
{
    private readonly IJobRepository _jobRepository = jobRepository;
    private readonly IAddressHandler _addressHandler = addressHandler;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <inheritdoc />
    public async Task<ServiceResult<Guid>> CreateAsync(JobDto jobDto, CancellationToken ct = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            var job = Job.Create(
                jobDto.ClientId,
                jobDto.Title,
                jobDto.Description,
                jobDto.PurchaseOrderNumber,
                jobDto.Priority,
                jobDto.Status,
                jobDto.ReceivedDate,
                jobDto.TargetStartDate,
                jobDto.Deadline,
                jobDto.CompletionDate
            );

            if (jobDto.SiteAddress is not null)
            {
                var address = await _addressHandler.GetOrCreateAddressAsync(jobDto.SiteAddress, ct);
                job.SiteAddress = address;
            }

            _jobRepository.Add(job);
            await _unitOfWork.CommitAsync(ct);

            await transaction.CommitAsync(ct);

            return ServiceResult<Guid>.Success(job.Id);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return ServiceResult<Guid>.Failure($"An error occurred while creating the job: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public async Task<ServiceResult<bool>> UpdateAsync(Guid guid, JobDto jobDto, CancellationToken ct = default)
    {
        var job = await _jobRepository.GetByIdAsync(guid, ct);
        if (job == null)
            return ServiceResult<bool>.Failure($"Job not found.");

        using var transaction = await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            job.Update(
                jobDto.Title,
                jobDto.Description,
                jobDto.PurchaseOrderNumber,
                jobDto.Priority,
                jobDto.Status,
                jobDto.ReceivedDate,
                jobDto.TargetStartDate,
                jobDto.Deadline,
                jobDto.CompletionDate
            );

            if (jobDto.SiteAddress is not null)
            {
                var address = await _addressHandler.GetOrCreateAddressAsync(jobDto.SiteAddress, ct);
                job.SiteAddress = address;
            }

            await _unitOfWork.CommitAsync(ct);
            await transaction.CommitAsync(ct);

            return ServiceResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return ServiceResult<bool>.Failure($"An error occurred while updating the job: {ex.Message}");
        }
    }
}
