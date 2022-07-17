namespace ImplementingGenericClasses.Repositories;

public class GenericRepository<T>
{
    protected readonly List<T> _items = new();

    public void Add(T item)
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