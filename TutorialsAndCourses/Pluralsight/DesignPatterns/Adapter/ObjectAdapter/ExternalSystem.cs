namespace Adapter.ObjectAdapter;

public class ExternalSystem
{
    public CityFromExternalSystem GetCity()
    {
        return new CityFromExternalSystem("Adelaide", "Radelaide", 1000000);
    }
}