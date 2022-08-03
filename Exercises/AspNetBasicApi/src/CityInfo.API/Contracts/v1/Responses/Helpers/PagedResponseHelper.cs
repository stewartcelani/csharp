using CityInfo.API.Contracts.v1.Requests.Queries;
using CityInfo.API.Domain.Filters;
using CityInfo.API.Services;

namespace CityInfo.API.Contracts.v1.Responses.Helpers;

public static class PagedResponseHelper
{
    public static PagedResponse<T> CreatePaginatedCityResponse<T>(IUriService uriService, PaginationFilter paginationFilter,
        List<T> response)
    {
        var nextPage = response.Count == paginationFilter.PageSize
            ? uriService
                .GetCitiesUri(new PaginationQuery(paginationFilter.PageNumber + 1, paginationFilter.PageSize))
                .ToString()
            : null;

        var previousPage = paginationFilter.PageNumber >= 2
            ? uriService.GetCitiesUri(
                new PaginationQuery(paginationFilter.PageNumber - 1, paginationFilter.PageSize)).ToString()
            : null;

        return new PagedResponse<T>
        {
            Data = response,
            PageNumber = paginationFilter.PageNumber,
            PageSize = paginationFilter.PageSize,
            NextPage = nextPage,
            PreviousPage = previousPage
        };
    }
}