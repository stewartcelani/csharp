using GenericMethodsAndDelegates.Entities;

namespace GenericMethodsAndDelegates.Repositories;

public interface IEmployeeRepository : IRepository<Employee, Guid>
{
}