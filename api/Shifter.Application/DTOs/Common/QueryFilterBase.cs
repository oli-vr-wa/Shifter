namespace Shifter.Application.DTOs.Common;

public abstract record QueryFilterBase(
    string? SearchTerm,
    int PageNumber,
    int PageSize,
    string? SortColumn,
    bool SortDescending)
{
    private const int MaxPageSize = 100;
    private const int DefaultPageSize = 20;


    public int PageNumber { get; } = Math.Max(1, PageNumber);

    public int PageSize { get; } = PageSize < 1 ? DefaultPageSize : Math.Min(PageSize, MaxPageSize);
}
