namespace Polymorphism.Entities;

public class Developer : Employee, IEntitlements
{
    public Developer(string name, int age) : base(name, age)
    {
    }

    public override void CalculateWage()
    {
        Console.WriteLine($"{nameof(Developer)}.{nameof(CalculateWage)}");
    }

    public void CalculateEntitlements()
    {
        Console.WriteLine($"{nameof(Developer)}.{nameof(CalculateEntitlements)}");
    }
}