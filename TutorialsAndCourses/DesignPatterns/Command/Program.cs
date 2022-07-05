using Command;

CommandManager commandManager = new();
IEmployeeManagerRepository repository = new EmployeeManagerRepository();

commandManager.Invoke(new AddEmployeeToManagerList(repository, 1, new Employee(111, "Kevin")));
commandManager.Invoke(new AddEmployeeToManagerList(repository, 1, new Employee(111, "Kevin")));
commandManager.Invoke(new AddEmployeeToManagerList(repository, 1, new Employee(222, "Clara")));
commandManager.Invoke(new AddEmployeeToManagerList(repository, 2, new Employee(333, "Tom")));
repository.WriteDataStore();
Console.WriteLine();

commandManager.Undo();
repository.WriteDataStore();
Console.WriteLine();

commandManager.UndoAll();
repository.WriteDataStore();

Console.ReadKey();

