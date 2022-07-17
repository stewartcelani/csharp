namespace ImplementingGenericClasses.Entities;

public class Organization : BaseEntity
{
    public string? Name { get; set; }
    
    public override string ToString() => $"Id: {Id}, Name: {Name}";
}