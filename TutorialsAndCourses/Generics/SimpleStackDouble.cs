namespace WiredBrainCoffee;

public class SimpleStackDouble
{
    private readonly List<double> _items = new();

    public int Count => _items.Count;

    public void Push(double item) => _items.Add(item);

    public double Pop()
    {
        var last = _items[^1];
        _items.Remove(last);
        return last;
    }
}