namespace Memento;

/// <summary>
/// Receiver (implementation)
/// </summary>
public class EmployeeManagerRepository : IEmployeeManagerRepository
{
    private List<Manager> _managers = new()
    {
        new Manager(1, "Katie"),
        new Manager(2, "Geoff")
    };
    
    
    public void AddEmployee(int managerId, Employee employee)
    {
        _managers.Single(m => m.Id == managerId).Employees.Add(employee);
    }

    public void RemoveEmployee(int managerId, Employee employee)
    {
        _managers.Single(m => m.Id == managerId).Employees.Remove(employee);
    }

    public bool HasEmployee(int managerId, int employeeId)
    {
        return _managers.Single(m => m.Id == managerId).Employees.Any(e => e.Id == employeeId);
    }

    public void WriteDataStore()
    {
        foreach (var manager in _managers)
        {
            Console.WriteLine($"Manager {manager.Id}, {manager.Name}");
            if (manager.Employees.Any())
            {
                foreach (var employee in manager.Employees)
                {
                    Console.WriteLine($"\t Employee {employee.Id}, {employee.Name}");
                }
            }
            else
            {
                Console.WriteLine($"\t No employees.");
            }
        }
    }
}