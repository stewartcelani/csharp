using CityInfo.API.Contracts.Requests.Queries;
using Microsoft.AspNetCore.WebUtilities;

namespace CityInfo.API.Services;

public class UriService : IUriService
{
    private readonly string _baseUri;

    public UriService(string baseUri)
    {
        _baseUri = baseUri;
    }
    
    public Uri GetCitiesUri(PaginationQuery? paginationQuery = null)
    {
        var uri = new Uri(_baseUri);

        if (paginationQuery is null) return uri;

        var modifiedUri =
            QueryHelpers.AddQueryString(uri.ToString(), "pageNumber", paginationQuery.PageNumber.ToString());
        modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", paginationQuery.PageSize.ToString());

        return new Uri(modifiedUri);
    }
}