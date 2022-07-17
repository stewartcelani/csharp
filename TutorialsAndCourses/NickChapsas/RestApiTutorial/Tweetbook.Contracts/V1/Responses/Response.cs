namespace Tweetbook.Contracts.V1.Responses;

public class Response<T>
{
    public Response()
    {
        // Needed by Refit SDK
    }

    public Response(T response)
    {
        Data = response;
    }

    public T Data { get; init; }
}