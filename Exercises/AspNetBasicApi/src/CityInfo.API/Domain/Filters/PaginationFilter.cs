namespace CityInfo.API.Domain.Filters;

public class PaginationFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 100;
}