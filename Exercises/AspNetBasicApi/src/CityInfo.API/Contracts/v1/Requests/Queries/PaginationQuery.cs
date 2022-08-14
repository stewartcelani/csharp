using System.Web;
using Newtonsoft.Json;

namespace CityInfo.API.Contracts.v1.Requests.Queries;

public class PaginationQuery
{
    public PaginationQuery()
    {
        PageNumber = 1;
        PageSize = 100;
    }

    public PaginationQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize > 100 ? 100 : pageSize;
    }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }
}

public static class PaginationQueryHelper
{
    public static string ToQueryString(this PaginationQuery paginationQuery)
    {
        var convert = JsonConvert.DeserializeObject<IDictionary<string, string>>(JsonConvert.SerializeObject(paginationQuery));
        
        return "?" + string.Join("&", (convert ?? throw new InvalidOperationException()).Select(x => HttpUtility.UrlEncode(x.Key) + "=" + HttpUtility.UrlEncode(x.Value)));
    }
}