namespace Polymorphism.Entities;

public class Manager : Employee, IEntitlements
{
    public Manager(string name, int age) : base(name, age)
    {
    }
    
    public override void PerformWork()
    {
        Console.WriteLine($"{nameof(Manager)}.{nameof(PerformWork)} (productivity: {Productivity})");
    }

    public override void CalculateWage()
    {
        Console.WriteLine($"{nameof(Manager)}.{nameof(CalculateWage)}");
    }

    public void AttendManagementMeeting()
    {
        Console.WriteLine($"{nameof(Manager)}.{nameof(AttendManagementMeeting)}: This could have been an email...");
    }

    public void CalculateEntitlements()
    {
        Console.WriteLine($"{nameof(Manager)}.{nameof(CalculateEntitlements)}");
    }
}