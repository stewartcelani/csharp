using GenericMethodsAndDelegates.Entities;

namespace GenericMethodsAndDelegates.Repositories;

public class ListRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    private readonly List<TEntity> _items = new();
    
    public TEntity GetById(TKey id) => _items.Single(i => EqualityComparer<TKey>.Default.Equals(i.Id, id));
    
    public IEnumerable<TEntity> GetAll()
    {
        return _items.ToList();
    }

    public void Add(TEntity item)
    {
        _items.Add(item);
    }

    public void Save()
    {
        foreach (var item in _items)
        {
            Console.WriteLine(item);
        }
    }

    public void Remove(TEntity item) => _items.Remove(item);
}