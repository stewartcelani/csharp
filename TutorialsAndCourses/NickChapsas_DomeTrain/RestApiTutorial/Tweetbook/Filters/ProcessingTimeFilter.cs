using Microsoft.AspNetCore.Mvc.Filters;

namespace Tweetbook.Filters;

public class ProcessingTimeFilter : IAsyncActionFilter
{
    private const string ProcessingTimeHeaderName = "x-processing-time";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var requestStart = DateTime.Now;
        await next();
        var timeElapsed = DateTime.Now - requestStart;
        context.HttpContext.Response.Headers.Add(ProcessingTimeHeaderName, $"{timeElapsed.Milliseconds.ToString()}ms");
    }
}