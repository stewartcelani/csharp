namespace Adapter.ObjectAdapter;

public class CityAdapter : ICityAdapter
{
    public ExternalSystem ExternalSystem { get; private set; } = new();
    
    public City GetCity()
    {
        // call into the external system
        var cityFromExternalSystem = ExternalSystem.GetCity();

        // adapt (or map) the CityFromExternalCity to a City
        return new City($"{cityFromExternalSystem.Name} - {cityFromExternalSystem.NickName}",
            cityFromExternalSystem.Inhabitants);
    }
}