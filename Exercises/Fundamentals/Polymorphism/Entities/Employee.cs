namespace Polymorphism.Entities;

public abstract class Employee
{
    public string Name { get; init; }
    public int Age { get; init; }
    public double Productivity { get; init; } = 1;

    protected Employee(string name, int age)
    {
        Name = name;
        Age = age;
    }
    
    public virtual void PerformWork()
    {
        Console.WriteLine($"{nameof(Employee)}.{nameof(PerformWork)} (productivity: {Productivity})");
    }

    public abstract void CalculateWage();

}