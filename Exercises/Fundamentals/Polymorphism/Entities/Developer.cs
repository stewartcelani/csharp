namespace Polymorphism.Entities;

public class Developer : Employee
{
    public Developer(string name, int age) : base(name, age)
    {
    }

    public override void CalculateWage()
    {
        Console.WriteLine($"{nameof(Developer)}.{nameof(CalculateWage)}");
    }
}