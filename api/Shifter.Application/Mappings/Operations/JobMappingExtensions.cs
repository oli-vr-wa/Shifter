using Shifter.Application.DTOs.Operations;
using Shifter.Application.Mappings.Location;
using Shifter.Core.Entities.Operations;
using System.Linq.Expressions;

namespace Shifter.Application.Mappings.Operations;

public static class JobMappingExtensions
{
    /// <summary>
    /// Creates a <see cref="JobDto"/> from a <see cref="Job"/>.
    /// </summary>
    /// <param name="job">The job to convert.</param>
    /// <returns>A <see cref="JobDto"/> containing the mapped job data, with null text fields converted to empty strings and the
    /// site address converted when present.</returns>
    public static JobDto ToDto(this Job job) => new(
        job.Id,
        job.ClientId,
        job.Title,
        job.Description ?? string.Empty,
        job.PurchaseOrderNumber ?? string.Empty,
        job.Priority,
        job.Status,
        job.ReceivedDate,
        job.TargetStartDate,
        job.Deadline,
        job.CompletionDate,
        job.SiteAddress?.ToDto()
    );

    /// <summary>
    /// Converts a job entity to a list item data transfer object.
    /// </summary>
    /// <remarks>Maps nullable text fields to empty strings and uses <see cref="DateTime.MinValue"/> when
    /// <c>ReceivedDate</c> is not set.</remarks>
    /// <param name="job">The job to convert.</param>
    /// <returns>A <c>JobListItemDto</c> containing list-display fields mapped from the specified job.</returns>
    public static JobListItemDto ToListItemDto(this Job job) => new(
        job.Id,
        job.Title,
        job.Description ?? string.Empty,
        job.PurchaseOrderNumber ?? string.Empty,
        job.Priority,
        job.Status,
        job.ReceivedDate ?? DateTime.MinValue,
        job.TargetStartDate,
        job.Deadline,
        job.CompletionDate,
        job.SiteAddress?.ShortAddress
    );

    /// <summary>
    /// Gets an expression that maps a <see cref="Job"/> to a <see cref="JobListItemDto"/> for list-item projection.
    /// </summary>
    /// <remarks>Normalizes nullable text fields to empty strings, uses <see cref="DateTime.MinValue"/> when
    /// <c>ReceivedDate</c> is null, and formats the site address into a single display string when available.</remarks>
    public static Expression<Func<Job, JobListItemDto>> ProjectToListItem
    {
        get
        {
            return job => new JobListItemDto(
                job.Id,
                job.Title,
                job.Description ?? string.Empty,
                job.PurchaseOrderNumber ?? string.Empty,
                job.Priority,
                job.Status,
                job.ReceivedDate ?? DateTime.MinValue,
                job.TargetStartDate,
                job.Deadline,
                job.CompletionDate,
                job.SiteAddress != null ? $"{job.SiteAddress.StreetNumber} {job.SiteAddress.StreetName}, {job.SiteAddress.Suburb}, {job.SiteAddress.State} {job.SiteAddress.PostCode}" : null
            );
        }
    }
}
