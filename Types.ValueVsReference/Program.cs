using Types.ValueVsReference;
/*
 * Value Types
 * - bool, int, decimal, double, float, byte, char, enum, struct
 * Reference Types
 * - class, interface, delegate, record, dynamic, object, string
 */

// REFERENCE TYPE - Class example
var computer = new Computer();
var laptop = computer;
Console.WriteLine($"Memory: {computer.Memory}"); // Memory: 8
Methods.IncreaseRam(computer);
Console.WriteLine($"Memory: {computer.Memory}"); // Memory: 16
Console.WriteLine($"Memory: {laptop.Memory}"); // Memory: 16
Console.WriteLine();

// VALUE TYPE - String
string s = "ABC";
Methods.ModifyString(s);
Console.WriteLine($"String s: {s}"); // String s: ABC
Methods.ModifyString(ref s);
Console.WriteLine($"String s: {s}"); // String s: ABCxyz

// VALUE TYPE - Int
int i = 1;
Methods.ModifyInt(i);
Console.WriteLine($"Int i: {i}"); // Int i: 1
Methods.ModifyInt(ref i);
Console.WriteLine($"Int i: {i}"); // Int i: 2


 
namespace Types.ValueVsReference
{
    public class Computer
    {
        public int Memory { get; set; } = 8;
    }

    public static class Methods
    {
        public static void IncreaseRam(Computer computer)
        {
            computer.Memory *= 2;
        }

        public static void ModifyString(string s)
        {
            // ReSharper disable once RedundantAssignment
            s += "xyz";
        }

        public static void ModifyString(ref string s)
        {
            s += "xyz";
        }

        public static void ModifyInt(int i)
        {
            // ReSharper disable once RedundantAssignment
            i += 1;
        }

        public static void ModifyInt(ref int i)
        {
            i++;
        }
    }
    

}