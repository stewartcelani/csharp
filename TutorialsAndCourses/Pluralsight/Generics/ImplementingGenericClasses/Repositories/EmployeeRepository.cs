using ImplementingGenericClasses.Entities;

namespace ImplementingGenericClasses.Repositories;

public class EmployeeRepository
{
    private readonly List<Employee> _employees = new();

    public void Add(Employee employee)
    {
        if (employee.Id == 0)
            employee.Id = _employees.Count + 1;
        _employees.Add(employee);
    }

    public void Save()
    {
        foreach (var employee in _employees)
        {
            Console.WriteLine(employee);
        }
    }
}