namespace UnderstandingTheNeedForGenerics;

public class SimpleStackString
{
    private readonly List<string> _items = new();

    public int Count => _items.Count;

    public void Push(string item) => _items.Add(item);

    public string Pop()
    {
        var last = _items[^1];
        _items.Remove(last);
        return last;
    }
}