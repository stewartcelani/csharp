using GenericMethodsAndDelegates.Entities;

namespace GenericMethodsAndDelegates.Repositories;

public interface IWriteRepository<in TEntity>// where TEntity : class, IEntity<TKey>
{
    void Add(TEntity item);
    void Remove(TEntity item);
    void Save();
}