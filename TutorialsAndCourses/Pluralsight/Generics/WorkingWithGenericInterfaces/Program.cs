using WorkingWithGenericInterfaces.Data;
using WorkingWithGenericInterfaces.Entities;
using WorkingWithGenericInterfaces.Repositories;

/*
 * Generic SqlRepository implementing generic IRepository with generic ID property type from generic IBaseEntity
 */
var sqlRepository = new SqlRepository<Employee, Guid>(new DataContext());
var calan = new Employee { FirstName = "Calan" };
sqlRepository.Add(calan);
sqlRepository.Add(new Employee { FirstName = "Bob" });
sqlRepository.Add(new Employee { FirstName = "Roger" });
sqlRepository.Save();
var employees = sqlRepository.GetAll();
foreach (var employee in employees)
{
    Console.WriteLine(employee);
}

var calanLookup = sqlRepository.GetById(calan.Id);
Console.WriteLine(calanLookup + "\n");

/*
 * Same with the ListRepository
 */
var listRepository = new ListRepository<Organization, Guid>();
var bakery = new Organization { Name = "Main St. Bakery" };
var officeSupplies = new Organization { Name = "OfficeWorks" };
listRepository.Add(bakery);
listRepository.Add(officeSupplies);
listRepository.Save();
var organizations = listRepository.GetAll();
foreach (var organization in organizations)
{
    Console.WriteLine(organization);
}

var bakeryLookup = listRepository.GetById(bakery.Id);
Console.WriteLine(bakeryLookup + "\n");


/*
 * Covariance - <out TEntity> - less derived type
 */
IReadOnlyRepository<IEntity<Guid>, Guid> repo = new SqlRepository<Employee, Guid>(new DataContext());
IEnumerable<IEntity<Guid>> entities = repo.GetAll();
var employeeList = entities.Where(x => x is Employee).Cast<Employee>().ToList();
foreach (var employee in employeeList)
{
    Console.WriteLine(employee);
}

/*
 * Contravariance - <in TEntity> - more derived type
 */
IWriteRepository<Manager, Guid> repository = new SqlRepository<Employee, Guid>(new DataContext());
var manager = new Manager { FirstName = "Gregory" };
repository.Add(manager);
repository.Add(new Manager { FirstName = "Tiffany" });
repository.Save();

Console.ReadLine();