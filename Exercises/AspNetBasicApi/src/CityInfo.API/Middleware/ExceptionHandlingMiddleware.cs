using CityInfo.API.Logging;

namespace CityInfo.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _request;
    
    private readonly ILoggerAdapter<ExceptionHandlingMiddleware> _logger;
    
    public ExceptionHandlingMiddleware(RequestDelegate request, ILoggerAdapter<ExceptionHandlingMiddleware> logger)
    {
        _request = request;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _request(context);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            context.Response.StatusCode = 500;
            await context.Response.CompleteAsync();
        }
    }
}