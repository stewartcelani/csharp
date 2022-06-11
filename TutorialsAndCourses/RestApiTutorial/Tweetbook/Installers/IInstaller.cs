namespace Tweetbook;

public interface IInstaller
{
    void InstallServices(IServiceCollection services, IConfiguration configuration);
}