using WorkingWithGenericInterfaces.Entities;

namespace WorkingWithGenericInterfaces.Repositories;

public interface IRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>, IWriteRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
}