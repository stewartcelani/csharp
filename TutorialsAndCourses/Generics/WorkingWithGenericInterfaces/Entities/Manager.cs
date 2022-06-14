namespace WorkingWithGenericInterfaces.Entities;

public class Manager : Employee
{
    public override string ToString() => base.ToString() + " (Manager)";
}