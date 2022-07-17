using GenericMethodsAndDelegates.Entities;

namespace GenericMethodsAndDelegates.Repositories;

public interface IReadOnlyRepository<out TEntity, in TKey> where TEntity : class, IEntity<TKey>
{
    TEntity GetById(TKey id);
    IEnumerable<TEntity> GetAll();
}