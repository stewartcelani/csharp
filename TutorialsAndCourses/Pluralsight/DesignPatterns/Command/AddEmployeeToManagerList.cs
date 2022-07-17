namespace Command;

/// <summary>
/// Concrete Command
/// </summary>
public class AddEmployeeToManagerList : ICommand
{
    private readonly IEmployeeManagerRepository _employeeManagerRepository;
    private readonly int _managerId;
    private readonly Employee _employee;

    public AddEmployeeToManagerList(IEmployeeManagerRepository employeeManagerRepository, int managerId, Employee employee)
    {
        _employeeManagerRepository = employeeManagerRepository;
        _managerId = managerId;
        _employee = employee;
    }

    public bool CanExecute()
    {
        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (_employeeManagerRepository.HasEmployee(_managerId, _employee.Id))
        {
            return false;
        }

        return true;
    }

    public void Execute()
    {
        _employeeManagerRepository.AddEmployee(_managerId, _employee);
    }

    public void Undo()
    {
        _employeeManagerRepository.RemoveEmployee(_managerId, _employee);
    }

}