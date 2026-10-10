namespace Shifter.Application.DTOs.Common;

public abstract record QueryFilterBase
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;

    public int PageNumber { get; init; } = 1;

    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public string? SortColumn { get; init; }
    public bool SortDescending { get; init; }

    public string? SearchTerm { get; init; }
}
