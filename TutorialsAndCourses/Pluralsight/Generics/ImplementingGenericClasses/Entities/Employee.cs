namespace ImplementingGenericClasses.Entities;

public class Employee : BaseEntity
{
    public string? FirstName { get; set; }

    public override string ToString() => $"Id: {Id}, FirstName: {FirstName}";
}