namespace FluentApiBuilder;

public class TwoLeggedApiClient :
    IClientIdSelectionStage,
    IClientSecretSelectionStage,
    IAccountIdSelectionStage,
    IOptionalConfigurationStage,
    IBuildStage
{
    private string _accountId;
    private string _clientId;
    private string _clientSecret;
    private ApiClientConfiguration _configuration;

    private TwoLeggedApiClient()
    {
        _configuration = new ApiClientConfiguration();
    }

    public IOptionalConfigurationStage ForAccount(string accountId)
    {
        _accountId = accountId;
        return this;
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

    public IBuildStage WithOptions(Action<ApiClientConfiguration> config)
    {
        var configuration = new ApiClientConfiguration();
        config?.Invoke(configuration);
        _configuration = configuration;
        return this;
    }

    public ApiClient Build()
    {
        return new ApiClient(_clientId, _clientSecret, _accountId, _configuration);
    }

    public static IClientIdSelectionStage Configure()
    {
        return new TwoLeggedApiClient();
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
    public IOptionalConfigurationStage ForAccount(string accountId);
}

public interface IOptionalConfigurationStage
{
    public IBuildStage WithOptions(Action<ApiClientConfiguration> config);
    public ApiClient Build();
}

public interface IBuildStage
{
    public ApiClient Build();
}

public class ApiClient
{
    private HttpClient _http = new();

    public ApiClient(string clientId, string clientSecret, string accountId, ApiClientConfiguration configuration)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
        AccountId = accountId;
        Configuration = configuration;
    }

    public string ClientId { get; }
    public string ClientSecret { get; }
    public string AccountId { get; }

    public ApiClientConfiguration Configuration { get; }

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