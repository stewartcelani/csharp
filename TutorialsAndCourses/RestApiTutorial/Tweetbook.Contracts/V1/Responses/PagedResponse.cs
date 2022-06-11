namespace Tweetbook.Contracts.V1.Responses;

public class PagedResponse<T>
{
    public PagedResponse()
    {
        // Needed by Refit SDK
    }

    public PagedResponse(IEnumerable<T> data)
    {
        Data = data;
    }

    public IEnumerable<T> Data { get; init; }

    public int? PageNumber { get; set; }

    public int? PageSize { get; set; }

    public string? NextPage { get; set; }

    public string? PreviousPage { get; set; }
}