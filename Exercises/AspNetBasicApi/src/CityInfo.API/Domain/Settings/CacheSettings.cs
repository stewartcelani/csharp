namespace CityInfo.API.Domain.Settings;

public class CacheSettings
{
    public bool Enabled { get; set; } = false;
    public string ConnectionString { get; set; }
}