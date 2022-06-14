namespace ImplementingGenericClasses.Repositories;

public class GenericRepositoryWithMultipleTypeParameters<TEntity, TKey>
{
    public TKey Key { get; set; }
    
    protected readonly List<TEntity> _items = new();

    public GenericRepositoryWithMultipleTypeParameters(TKey key)
    {
        Key = key;
    }

    public void Add(TEntity item)
    {
        
        /*if (T.Id == 0)
            T.Id = _list.Count + 1;*/
        _items.Add(item);
    }

    public void Save()
    {
        foreach (var item in _items)
        {
            Console.WriteLine(item);
        }
    }
}