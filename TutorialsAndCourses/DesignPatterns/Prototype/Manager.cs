using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace Prototype;

public class Manager : IPerson
{
    public string Name { get; set; }

    public Manager(string name)
    {
        Name = name;
    }
    
    public IPerson Clone(bool deepClone = false)
    {
        if (!deepClone) return (IPerson)MemberwiseClone();

        var objectAsJson = JsonConvert.SerializeObject(this);
        return (IPerson)JsonConvert.DeserializeObject<Manager>(objectAsJson);
    }
}