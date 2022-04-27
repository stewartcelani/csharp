namespace Patterns.FluentApiBuilder;

public class TwoLeggedApiClient : 
    IClientIdSelectionStage, 
    IClientSecretSelectionStage, 
    IAccountIdSelectionStage,
    ICreateApiClientStage
{
    private string _clientId;
    private string _clientSecret;
    private string _accountId;
    private ApiClientConfiguration _configuration;

    private TwoLeggedApiClient(ApiClientConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static IClientIdSelectionStage Configure()
    {
        var configuration = new ApiClientConfiguration();
        return new TwoLeggedApiClient(configuration);
    }
    
    public static IClientIdSelectionStage Configure(Action<ApiClientConfiguration> config)
    {
        var configuration = new ApiClientConfiguration();
        config?.Invoke(configuration);
        return new TwoLeggedApiClient(configuration);
    }

    public IClientSecretSelectionStage WithClientId(string clientId)
    {
        _clientId = clientId;
        return this;
    }

    public IAccountIdSelectionStage AndClientSecret(string clientSecret)
    {
        _clientSecret = clientSecret;
        return this;
    }

    public ICreateApiClientStage ForAccount(string accountId)
    {
        _accountId = accountId;
        return this;
    }

    public ApiClient CreateApiClient()
    {
        return new ApiClient(_clientId, _clientSecret, _accountId, _configuration);
    }
}

public interface IClientIdSelectionStage
{
    public IClientSecretSelectionStage WithClientId(string clientId);
}

public interface IClientSecretSelectionStage
{
    public IAccountIdSelectionStage AndClientSecret(string clientSecret);
}

public interface IAccountIdSelectionStage
{
    public ICreateApiClientStage ForAccount(string accountId);
}

public interface ICreateApiClientStage
{
    public ApiClient CreateApiClient();
}

public class ApiClient
{
    public string ClientId { get; }
    public string ClientSecret { get; }
    public string AccountId { get; }

    public ApiClientConfiguration Configuration { get; }
    
    private HttpClient _http = new();
    
    public ApiClient(string clientId, string clientSecret, string accountId, ApiClientConfiguration configuration)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
        AccountId = accountId;
        Configuration = configuration;
    }

    public void Example()
    {
        Configuration.LoggingMethod?.Invoke("Example of custom logging passed via lambda config.");
    }
}

public class ApiClientConfiguration
{
    public string ApiClientName { get; set; } = "Default";
    public int MaxRetries { get; set; } = 4;
    public int SecondsBetweenRetries { get; set; } = 15;
    public int DegreesOfParallelism { get; set; } = 4;
    public Action<string>? LoggingMethod { get; set; }
}