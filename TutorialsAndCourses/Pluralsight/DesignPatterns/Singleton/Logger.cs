namespace Singleton;

public class Logger
{
    private static Logger? _instance;

    public static Logger Instance => _instance ??= new Logger();

    protected Logger(){}

    public void Log(string message)
    {
        Console.WriteLine(message);
    }

}