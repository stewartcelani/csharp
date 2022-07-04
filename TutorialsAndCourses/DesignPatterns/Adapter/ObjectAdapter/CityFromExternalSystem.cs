namespace Adapter.ObjectAdapter;

public class CityFromExternalSystem
{
    public string Name { get; private set; }
    public string NickName { get; private set; }
    public int Inhabitants { get; private set; }

    public CityFromExternalSystem(string name, string nickname, int inhabitants)
    {
        Name = name;
        NickName = nickname;
        Inhabitants = inhabitants;
    }
    
}