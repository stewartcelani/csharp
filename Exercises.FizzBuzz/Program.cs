/*
 * Write a program that prints the numbers from 1 to 100.
 * But for multiples of three print “Fizz” instead of the number and for the multiples of five print “Buzz”.
 * For numbers which are multiples of both three and five print “FizzBuzz”.
 */

using System.Text;

for (int i = 1; i <= 100; i++)
{
    var sb = new StringBuilder();

    if (i % 3 == 0)
    {
        sb.Append("Fizz");
    }

    if (i % 5 == 0)
    {
        sb.Append("Buzz");
    }

    if (sb.Length == 0)
    {
        sb.Append($"{i}");
    }
    
    Console.WriteLine(sb.ToString());
}