using CityInfo.API.Contracts.v1;
using CityInfo.API.Contracts.v1.Requests.Queries;
using Microsoft.AspNetCore.WebUtilities;

namespace CityInfo.API.Services;

public class UriService : IUriService
{
    private readonly string _baseUri;
    
    public UriService(string baseUri)
    {
        _baseUri = baseUri ?? throw new NullReferenceException(nameof(baseUri));
    }
    
    public Uri GetCitiesUri(PaginationQuery? paginationQuery = null)
    {
        var uri = new Uri($"{_baseUri}{ApiRoutesV1.Cities.GetAll.Url}");

        if (paginationQuery is null) return uri;

        var modifiedUri =
            QueryHelpers.AddQueryString(uri.ToString(), "pageNumber", paginationQuery.PageNumber.ToString());
        modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", paginationQuery.PageSize.ToString());

        return new Uri(modifiedUri);
    }
}