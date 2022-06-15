/*
 * Static Members in generic classes
 */

using System.Runtime.CompilerServices;

var container = new Container<string>();
_ = new Container<string>();
_ = new Container<int>();

Console.WriteLine($"Container<string>: {Container<string>.InstanceCount}");
Console.WriteLine($"Container<int>: {Container<int>.InstanceCount}");
Console.WriteLine($"Container<bool>: {Container<bool>.InstanceCount}");
Console.WriteLine($"ContainerBase: {ContainerBase.InstanceCountBase}");

/*
 * Generic Methods in generic classes
 */
container.PrintItem("Test");
container.PrintItem<int>(1);

/*
 * Generics and mathematical operators
 */
Console.WriteLine(Math.Add(2, 3)); // 5
Console.WriteLine(Math.AddDodgy(2.2, 3.3)); // 5.5 - Dynamic dodgy, not type safe at runtime
// The type safe way to get an add method working with Int and double is to make an overload for add (copy-paste approach)
Console.WriteLine(Math.Add(2.3, 3.4)); // 5.7


public static class Math
{
    public static int Add(int x, int y) => x + y;

    public static T AddDodgy<T>(T x, T y) where T: notnull
    {
        dynamic a = x;
        dynamic b = y;
        return a + b;
    }

    public static double Add(double x, double y) => x + y;
}