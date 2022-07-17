using Singleton;

/*
 * Naive implementation
 */
var logger1 = Logger.Instance;
var logger2 = Logger.Instance;

if (logger1 == logger2 && logger2 == Logger.Instance)
{
    Console.WriteLine("Instances are the same.");
}

logger1.Log("Log message from logger1.");
logger2.Log("Log message from logger2.");

Console.WriteLine();

/*
 * Improved implementation using .NET inbuilt Lazy<T>
 * https://docs.microsoft.com/en-us/dotnet/api/system.lazy-1?view=net-6.0
 */

var lazyLogger1 = LazyLogger.Instance;
var lazyLogger2 = LazyLogger.Instance;

if (lazyLogger1 == lazyLogger2 && lazyLogger2 == LazyLogger.Instance)
{
    Console.WriteLine("Instances are the same.");
}

lazyLogger1.Log("Log message from lazyLogger1.");
lazyLogger2.Log("Log message from lazyLogger2.");

Console.ReadLine();