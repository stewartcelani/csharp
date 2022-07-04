namespace Adapter.ClassAdapter;

public class CityAdapter : ExternalSystem, ICityAdapter
{
    
    public City GetCity()
    {
        // call into the external system
        var cityFromExternalSystem = base.GetCity();

        // adapt (or map) the CityFromExternalCity to a City
        return new City($"{cityFromExternalSystem.Name} - {cityFromExternalSystem.NickName}",
            cityFromExternalSystem.Inhabitants);
    }
}