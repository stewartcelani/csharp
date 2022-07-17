using GenericMethodsAndDelegates.Entities;
using GenericMethodsAndDelegates.Repositories;

public static class Examples
{
    public static void AddOrginizations(IRepository<Organization, Guid> repository, Organization[] organizations)
    {
        foreach (var organization in organizations)
        {
            repository.Add(organization);
        }
        repository.Save();
    }

    public static void AddBatch<TEntity>(IWriteRepository<TEntity> repository, IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            repository.Add(entity);
        }
        repository.Save();
    }

    public static void AddManagers(IWriteRepository<Manager> managerRepository)
    {
        var saraManager = new Manager() { FirstName = "Sara" };
        var saraManagerCopy = saraManager.Copy();
        saraManagerCopy.Id = Guid.NewGuid();
        managerRepository.Add(saraManager);
        managerRepository.Add(saraManagerCopy);
        managerRepository.Save();
    }

    public static void LogAdded<TEntity>(TEntity added)
    {
        Console.WriteLine($"{added?.GetType().Name} added: " + added);
    }

}