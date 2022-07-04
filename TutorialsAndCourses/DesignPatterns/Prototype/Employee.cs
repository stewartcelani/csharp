using Newtonsoft.Json;

namespace Prototype;

public class Employee : IPerson
{
    public Manager Manager { get; set; }
    public string Name { get; set; }

    public Employee(string name, Manager manager)
    {
        Name = name;
        Manager = manager;
    }
    
    public IPerson Clone(bool deepClone = false)
    {
        if (!deepClone) return (IPerson)MemberwiseClone();

        var objectAsJson = JsonConvert.SerializeObject(this);
        return (IPerson)JsonConvert.DeserializeObject<Employee>(objectAsJson);
    }
}