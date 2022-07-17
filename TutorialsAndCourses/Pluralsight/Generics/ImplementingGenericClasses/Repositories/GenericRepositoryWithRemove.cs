using ImplementingGenericClasses.Entities;

namespace ImplementingGenericClasses.Repositories;

public class GenericRepositoryWithRemove<T> : GenericRepository<T>
{
    public void Remove(T item) => _items.Remove(item);
}