using WorkingWithGenericInterfaces.Entities;

namespace WorkingWithGenericInterfaces.Repositories;

public interface IWriteRepository<in TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    void Add(TEntity item);
    void Remove(TEntity item);
    void Save();
}