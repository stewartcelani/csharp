namespace Polymorphism.Entities;

public sealed class JuniorDeveloper : Developer
{
    public JuniorDeveloper(string name, int age, double productivity) : base(name, age)
    {
        Productivity = productivity;
    }
    
    public override void PerformWork()
    {
        Console.WriteLine($"{nameof(JuniorDeveloper)}.{nameof(PerformWork)} (productivity: {Productivity})");
        base.PerformWork();
    }
    
    public override void CalculateWage()
    {
        Console.WriteLine($"{nameof(JuniorDeveloper)}.{nameof(CalculateWage)}");
    }

}