using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Tweetbook.Contracts.HealthChecks;

namespace Tweetbook.Helpers;

public static class HealthCheckHelper
{
    public static HealthCheckOptions Options()
    {
        return new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var response = new HealthCheckResponse
                {
                    Status = report.Status.ToString(),
                    Checks = report.Entries.Select(x => new HealthCheck
                    {
                        Component = x.Key,
                        Status = x.Value.Status.ToString(),
                        Description = x.Value.Description
                    }),
                    Duration = report.TotalDuration
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        };
    }
}