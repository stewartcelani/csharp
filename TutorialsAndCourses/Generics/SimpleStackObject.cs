namespace WiredBrainCoffee;

public class SimpleStackObject
{
    private readonly List<object> _items = new();

    public int Count => _items.Count;

    public void Push(object item) => _items.Add(item);

    public object Pop()
    {
        var last = _items[^1];
        _items.Remove(last);
        return last;
    }
}