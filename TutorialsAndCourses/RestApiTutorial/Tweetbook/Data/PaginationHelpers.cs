using Tweetbook.Contracts.V1.Requests.Queries;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Services;

namespace Tweetbook.Helpers;

public static class PaginationHelpers
{
    public static PagedResponse<T> CreatePaginatedResponse<T>(IUriService uriService, PaginationFilter paginationFilter,
        List<T> response)
    {
        var nextPage = response.Count == paginationFilter.PageSize
            ? uriService
                .GetAllPostsUri(new PaginationQuery(paginationFilter.PageNumber + 1, paginationFilter.PageSize))
                .ToString()
            : null;

        var previousPage = paginationFilter.PageNumber >= 2
            ? uriService.GetAllPostsUri(
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