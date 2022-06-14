namespace ImplementingGenericClasses.Repositories;

public class GenericRepositoryWithMultipleTypeParametersWithRemove<TEntity, TKey> : GenericRepositoryWithMultipleTypeParameters<TEntity, TKey>
{
    public void Remove(TEntity item) => _items.Remove(item);

    public GenericRepositoryWithMultipleTypeParametersWithRemove(TKey key) : base(key)
    {
    }
}