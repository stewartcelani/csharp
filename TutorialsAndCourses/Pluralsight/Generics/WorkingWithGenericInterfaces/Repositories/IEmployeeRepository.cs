using WorkingWithGenericInterfaces.Entities;

namespace WorkingWithGenericInterfaces.Repositories;

public interface IEmployeeRepository : IRepository<Employee, Guid>
{
}