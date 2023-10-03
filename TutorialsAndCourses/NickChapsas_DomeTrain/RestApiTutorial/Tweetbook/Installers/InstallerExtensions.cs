namespace Tweetbook.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddDbContext(this WebApplicationBuilder webApplicationBuilder)
    {
        new DbInstaller().InstallServices(webApplicationBuilder.Services, webApplicationBuilder.Configuration);
    }

    public static void AddMvc(this WebApplicationBuilder webApplicationBuilder)
    {
        new MvcInstaller().InstallServices(webApplicationBuilder.Services, webApplicationBuilder.Configuration);
    }

    public static void AddSwagger(this WebApplicationBuilder webApplicationBuilder)
    {
        new SwaggerInstaller().InstallServices(webApplicationBuilder.Services, webApplicationBuilder.Configuration);
    }

    public static void AddAuth(this WebApplicationBuilder webApplicationBuilder)
    {
        new AuthInstaller().InstallServices(webApplicationBuilder.Services, webApplicationBuilder.Configuration);
    }

    public static void AddCache(this WebApplicationBuilder webApplicationBuilder)
    {
        new CacheInstaller().InstallServices(webApplicationBuilder.Services, webApplicationBuilder.Configuration);
    }

    public static void AddHealthChecks(this WebApplicationBuilder webApplicationBuilder)
    {
        new HealthChecksInstaller().InstallServices(webApplicationBuilder.Services,
            webApplicationBuilder.Configuration);
    }
}