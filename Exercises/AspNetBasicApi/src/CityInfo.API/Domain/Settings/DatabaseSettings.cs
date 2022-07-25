namespace CityInfo.API.Domain.Settings;

public class DatabaseSettings
{
    public string ConnectionString { get; set; }
    public bool EnableSensitiveDataLogging { get; init; } = false;
}