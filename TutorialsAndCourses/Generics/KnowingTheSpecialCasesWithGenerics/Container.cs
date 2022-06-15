public class Container<T> : ContainerBase
{
    public Container() => InstanceCount++;
    public static int InstanceCount { get; private set; }
    public void PrintItem<TItem>(TItem item) => Console.WriteLine($"Item: {item}");
}