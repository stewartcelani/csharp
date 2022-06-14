using GenericMethodsAndDelegates.Data;
using GenericMethodsAndDelegates.Entities;
using GenericMethodsAndDelegates.Repositories;

/*
 * Non-generic implementation of AddOrginizations
 */
var orginizationRepository1 = new SqlRepository<Organization, Guid>(new DataContext());
Examples.AddOrginizations(orginizationRepository1, new[]
{
    new Organization { Name = "Valley Bakers" },
    new Organization { Name = "Mario's Pizza" }
});

/*
 * Generic implementation of AddBatch
 * The <Orginization> after AddBatch is below is actually redundant
 */
var orginizationRepository2 = new SqlRepository<Organization, Guid>(new DataContext());
Examples.AddBatch<Organization>(orginizationRepository2, new[]
{
    new Organization { Name = "Bridge St. Tyres" },
    new Organization { Name = "Universe Fitness" }
});
var employeeRepository1 = new SqlRepository<Employee, Guid>(new DataContext());
Examples.AddBatch(employeeRepository1, new[]
{
    new Employee { FirstName = "Tom" },
    new Employee { FirstName = "Sarah" }
});

/*
 * AddBatch as a Generic Extension Method (from Repositories/RepositoryExtensions.cs)
 */
var employeeRepository2 = new SqlRepository<Employee, Guid>(new DataContext());
employeeRepository2.AddRange(new[]
{
    new Employee { FirstName = "Margaret " },
    new Employee { FirstName = "Xavier" }
});

/*
 * Using a generic class copy method
 */
var employeeRepository3 = new SqlRepository<Employee, Guid>(new DataContext());
Examples.AddManagers(employeeRepository3);

/*
 * Using an ItemAdded delegate on the SqlRepository which will log when items are added if it is not null
 */
var itemAddedCallback = new ItemAdded<Employee>(Examples.LogAdded);
var employeeRepository4 = new SqlRepository<Employee, Guid>(new DataContext(), itemAddedCallback);
employeeRepository4.Add(new Employee
    { FirstName = "Nick" }); // Output: Employee added: Id: a15614a5-91dc-465b-b6d8-e3cd01766666, FirstName: Nick
var employeeRepository5 =
    new SqlRepository<Employee, Guid>(new DataContext(),
        Examples.LogAdded); // Can just use method name that has same signature as the delegate
employeeRepository5.Add(new Employee
    { FirstName = "Susan" }); // Output: Employee added: Id: 687c97ce-a067-41f2-aaeb-df64f8df573c, FirstName: Susan

/*
 * Declaring the delegate implicitly inline
 */
var employeeRepository6 = new SqlRepository<Employee, Guid>(new DataContext(),
    employee => Console.WriteLine($"Employee added: {employee.FirstName}"));
employeeRepository6.Add(new Manager
{
    FirstName = "Scott"
}); // Output: Employee added: Scott

/*
 * The ItemAdded delegate has the same signature as the BCL Action<T> delegate
 */
var employeeRepository7 = new SqlRepository<Employee, Guid>(new DataContext(), null,
    employee => Console.WriteLine($"Employee added using generic Action<T> delegate: {employee.FirstName}"));
employeeRepository7.Add(new Employee { FirstName = "Gregorovich" }); // Output: Employee added using generic Action<T> delegate: Gregorovich

/*
 * Events with EventHandler<T>
 */
var employeeRepository8 = new SqlRepository<Employee, Guid>(new DataContext());
employeeRepository8.ItemAdded += (sender, employee) => Console.WriteLine($"Employee added event (sent from {sender?.GetType().Name}): " + employee.FirstName);
employeeRepository8.Add(new Employee {FirstName = "Steve"}); // Output: Employee added event (sent from SqlRepository`2): Steve

Console.ReadLine();