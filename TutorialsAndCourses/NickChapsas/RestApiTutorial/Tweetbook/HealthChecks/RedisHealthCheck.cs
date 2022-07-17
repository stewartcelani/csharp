using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace Tweetbook.Helpers;

public class RedisHealthCheck : IHealthCheck
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisHealthCheck(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        try
        {
            var database = _connectionMultiplexer.GetDatabase();
            database.StringGet("health");
            return Task.FromResult(HealthCheckResult.Healthy());
        }
        catch (Exception e)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy(e.Message));
        }
    }
}