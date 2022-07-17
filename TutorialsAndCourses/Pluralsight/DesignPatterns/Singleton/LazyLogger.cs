namespace Singleton;

public class LazyLogger
{
    private static readonly Lazy<LazyLogger> _logger = new(() => new LazyLogger());

    public static LazyLogger Instance => _logger.Value;

    protected LazyLogger(){}

    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}