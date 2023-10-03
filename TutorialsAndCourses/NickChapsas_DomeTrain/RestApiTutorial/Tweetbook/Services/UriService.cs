using Microsoft.AspNetCore.WebUtilities;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests.Queries;

namespace Tweetbook.Services;

public class UriService : IUriService
{
    private readonly string _baseUri;

    public UriService(string baseUri)
    {
        _baseUri = baseUri;
    }

    public Uri GetPostUri(string postId)
    {
        return new Uri(_baseUri + ApiRoutes.Posts.Get.Replace("{postId}", postId));
    }

    public Uri GetAllPostsUri(PaginationQuery? paginationQuery = null)
    {
        var uri = new Uri(_baseUri);

        if (paginationQuery is null) return uri;

        var modifiedUri =
            QueryHelpers.AddQueryString(uri.ToString(), "pageNumber", paginationQuery.PageNumber.ToString());
        modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", paginationQuery.PageSize.ToString());

        return new Uri(modifiedUri);
    }
}