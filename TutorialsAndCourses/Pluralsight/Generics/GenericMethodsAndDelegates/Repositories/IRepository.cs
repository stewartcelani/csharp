using GenericMethodsAndDelegates.Entities;

namespace GenericMethodsAndDelegates.Repositories;

public interface IRepository<TEntity, in TKey> : IReadOnlyRepository<TEntity, TKey>, IWriteRepository<TEntity>
    where TEntity : class, IEntity<TKey>
{
}