using Polymorphism.Entities;

/*
 * Examples
 */
var developer = new Developer("Bob McDonald", 36);
developer.PerformWork(); // Employee.PerformWork (productivity: 1)
developer.CalculateWage(); // Developer.CalculateWage

var juniorDeveloper = new JuniorDeveloper("Josh Simpson", 22, 0.6);
juniorDeveloper.PerformWork(); // JuniorDeveloper.PerformWork (productivity: 0.6) \n Employee.PerformWork (productivity: 0.6)
juniorDeveloper.CalculateWage(); // JuniorDeveloper.CalculateWage

var manager = new Manager("Amelia Hamilton", 48);
manager.AttendManagementMeeting(); // Manager.AttendManagementMeeting: This could have been an email...
manager.PerformWork(); // Manager.PerformWork (productivity: 1)
manager.CalculateWage(); // Manager.CalculateWage

var experiencedManager = new Manager("Tom Smith", 55)
{
    Productivity = 1.1
};
experiencedManager.PerformWork(); // Manager.PerformWork (productivity: 1.1)

/*
 * How to find derived type in a collection of base types
 */
var employeeList = new List<Employee>
{
    developer,
    juniorDeveloper,
    manager,
    experiencedManager
};

/*
 * Below managers will still be a list of <Employee> and you'd be unable to call
 * manager.AttendManagementMeeting() if looping over the list
 */
var managers = employeeList.Where(x => x.GetType() == typeof(Manager)); // Count: 2

/*
 * Pattern matching
 */
var developers = employeeList.Where(x => x is Developer);

/*
 * Example of casting result of where query directly to the type of IEnumerable you need
 */
var juniorDevelopers = employeeList.Where(x => x is JuniorDeveloper).Cast<JuniorDeveloper>();

/*
 * Example of casting from IEnumerable<Employee> directly to IEnumerable<Manager> within a foreach declaration
 */
foreach (var m in employeeList.Where(x => x is Manager).Cast<Manager>())
{
    Console.WriteLine(m.Name);
    m.AttendManagementMeeting();
}


Console.ReadLine();