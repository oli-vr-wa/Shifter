using Shifter.Application.DTOs.Common;
using Shifter.Core.Entities.Operations;

namespace Shifter.Application.DTOs.Operations;

public record JobQueryFilter(
    string? SearchTerm = null,
    int PageNumber = 1,
    int PageSize = 20,
    string? SortColumn = null,
    bool SortDescending = false,
    Guid? ClientId = null,
    JobStatus? Status = null,
    JobPriority? Priority = null)
    : QueryFilterBase(SearchTerm, PageNumber, PageSize, SortColumn, SortDescending);
