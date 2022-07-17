namespace GenericMethodsAndDelegates.Repositories;

public static class RepositoryExtensions
{
    public static void AddRange<TEntity>(this IWriteRepository<TEntity> repository, IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            repository.Add(entity);
        }
        repository.Save();
    }
}