using Tweetbook.Data;
using Tweetbook.Helpers;

namespace Tweetbook;

public class HealthChecksInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<DataContext>("MSSQL")
            .AddCheck<RedisHealthCheck>("Redis");
    }
}