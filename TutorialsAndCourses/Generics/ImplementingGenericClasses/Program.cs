using System.Threading.Channels;
using ImplementingGenericClasses.Entities;
using ImplementingGenericClasses.Repositories;

/*
 * Employee Repository
 */
var george = new Employee { FirstName = "George" };
var emma = new Employee { FirstName = "Emma" };
var employeeRepository = new EmployeeRepository();
employeeRepository.Add(george);
employeeRepository.Add(emma);
employeeRepository.Save();
Console.WriteLine();

/*
 * GenericRepository<T>
 */
var bakery1 = new Organization { Name = "Main St. Bakery" };
var officeSupplies1 = new Organization { Name = "OfficeWorks" };
var organizationRepository = new GenericRepository<Organization>();
organizationRepository.Add(bakery1);
organizationRepository.Add(officeSupplies1);
organizationRepository.Save();
Console.WriteLine();

/*
 * GenericRepository<T> which inherits from another generic repository
 */
var bakery2 = new Organization { Name = "Main St. Bakery" };
var officeSupplies2 = new Organization { Name = "OfficeWorks" };
var organizationRepositoryWithRemove = new GenericRepositoryWithRemove<Organization>();
organizationRepositoryWithRemove.Add(bakery2);
organizationRepositoryWithRemove.Add(officeSupplies2);
organizationRepositoryWithRemove.Save();
Console.WriteLine();
organizationRepositoryWithRemove.Remove(bakery2);
organizationRepositoryWithRemove.Save();
Console.WriteLine();

/*
 * Multiple type parameters & inheritance
 */
var employeeRepositoryWithMultipleTypeParameters =
 new GenericRepositoryWithMultipleTypeParametersWithRemove<Employee, Guid>(Guid.NewGuid());
employeeRepositoryWithMultipleTypeParameters.Add(george);
employeeRepositoryWithMultipleTypeParameters.Add(emma);
employeeRepositoryWithMultipleTypeParameters.Save();
Console.WriteLine();
employeeRepositoryWithMultipleTypeParameters.Remove(george);
employeeRepositoryWithMultipleTypeParameters.Save();
Console.WriteLine();

/*
 * With type constraint of BaseEntity so that can use the Id property
 */
var genericRepository = new Repository<Employee>();
genericRepository.Add(new Employee{ FirstName = "Carla"});
genericRepository.Add(new Employee{ FirstName = "Roger"});
genericRepository.Save();
var roger = genericRepository.GetById(2);
Console.WriteLine(roger);
genericRepository.Remove(roger);
genericRepository.Save();
Console.WriteLine();
