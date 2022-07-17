namespace Tweetbook.Extensions;

public static class HttpContextExtensions
{
    public static string GetBaseUrl(this HttpContext httpContext)
    {
        return $"{httpContext.Request.Scheme}://{httpContext.Request.Host.ToUriComponent()}";
    }

    public static string GetUserId(this HttpContext httpContext)
    {
        return httpContext.User.Claims.Single(x => x.Type == "id")?.Value ?? string.Empty;
    }
}