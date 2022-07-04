namespace Adapter.ObjectAdapter;

public class City
{
    public string FullName { get; private set; }
    public long Inhabitants { get; private set; }

    public City(string fullName, long inhabitants)
    {
        FullName = fullName;
        Inhabitants = inhabitants;
    }
}