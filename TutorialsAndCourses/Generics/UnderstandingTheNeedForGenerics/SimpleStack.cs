namespace UnderstandingTheNeedForGenerics;

public class SimpleStack<T>
{
    private readonly List<T> _items = new();

    public int Count => _items.Count;

    public void Push(T item) => _items.Add(item);

    public T Pop()
    {
        var last = _items[^1];
        _items.Remove(last);
        return last;
    }
}