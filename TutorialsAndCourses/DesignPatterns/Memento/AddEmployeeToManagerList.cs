namespace Memento;

/// <summary>
/// Concrete Command & Originator
/// </summary>
public class AddEmployeeToManagerList : ICommand
{
    private readonly IEmployeeManagerRepository _employeeManagerRepository;
    private int _managerId;
    private Employee _employee;

    public AddEmployeeToManagerList(IEmployeeManagerRepository employeeManagerRepository, int managerId, Employee employee)
    {
        _employeeManagerRepository = employeeManagerRepository;
        _managerId = managerId;
        _employee = employee;
    }

    public AddEmployeeToManagerListMemento CreateMemento()
    {
        return new AddEmployeeToManagerListMemento(_managerId, _employee);
    }

    public void RestoreMemento(AddEmployeeToManagerListMemento memento)
    {
        _managerId = memento.ManagerId;
        _employee = memento.Employee;
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