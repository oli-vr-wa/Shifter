using Shifter.Application.DTOs.Common;
using Shifter.Core.Entities.Customers;

namespace Shifter.Application.DTOs.Customers;

public record ClientQueryFilter(
    string? SearchTerm = null,
    int PageNumber = 1,
    int PageSize = 20,
    string? SortColumn = null,
    bool SortDescending = false,
    ClientType? ClientType = null,
    bool? HasActiveJobs = null) 
    : QueryFilterBase(SearchTerm, PageNumber, PageSize, SortColumn, SortDescending);

