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
    private const int MaxPageNumber = 100;


    public int PageNumber { get; } = Math.Clamp(PageNumber, 1, MaxPageNumber);

    public int PageSize { get; } = PageSize < 1 ? DefaultPageSize : Math.Min(PageSize, MaxPageSize);

    public int Skip => (PageNumber - 1) * PageSize;

    /// <summary>
    /// Gets the search term, trimmed, or null if the search term is null or whitespace.
    /// </summary>
    public string? SearchTerm { get; } = string.IsNullOrWhiteSpace(SearchTerm) 
        ? null 
        : SearchTerm.Trim().ToLowerInvariant();

    /// <summary>
    /// Gets the sort column, trimmed, or null if the sort column is null or whitespace.
    /// </summary>
    public string? SortColumn { get; } = string.IsNullOrWhiteSpace(SortColumn)
        ? null
        : SortColumn.Trim().ToLowerInvariant();
}
