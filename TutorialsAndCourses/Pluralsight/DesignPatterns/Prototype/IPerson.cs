namespace Prototype;

public interface IPerson
{
    string Name { get; set; }
    IPerson Clone(bool deepClone);
}