using FluentValidation.AspNetCore;
using Tweetbook.Filters;
using Tweetbook.Services;

namespace Tweetbook;

public class MvcInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();
        services.AddEndpointsApiExplorer();

        services.AddAutoMapper(typeof(Program));

        services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add<ValidationFilter>();
                options.Filters.Add<ProcessingTimeFilter>();
            })
            .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Program>());

        services.AddScoped<IUriService>(provider =>
        {
            var accessor = provider.GetRequiredService<IHttpContextAccessor>();
            var request = accessor.HttpContext.Request;
            var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/");
            return new UriService(absoluteUri);
        });
    }
}