using ImplementingGenericClasses.Entities;

namespace ImplementingGenericClasses.Repositories;

public class Repository<T> where T : IBaseEntity
{
    private readonly List<T> _items = new();
    
    public T GetById(int id) => _items.Single(i => i.Id == id);

    public void Add(T item)
    {
        item.Id = _items.Any() ? _items.Max(i => i.Id) + 1 : 1;
        _items.Add(item);
    }

    public void Save()
    {
        foreach (var item in _items)
        {
            Console.WriteLine(item);
        }
    }

    public void Remove(T item) => _items.Remove(item);
}