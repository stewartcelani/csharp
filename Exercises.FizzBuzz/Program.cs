/*
 * Write a program that prints the numbers from 1 to 100.
 * But for multiples of three print “Fizz” instead of the number and for the multiples of five print “Buzz”.
 * For numbers which are multiples of both three and five print “FizzBuzz”.
 */
using Exercises.FizzBuzz;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Benchmark>();

namespace Exercises.FizzBuzz
{
    [MemoryDiagnoser()]
    public class Benchmark
    {
        [Benchmark] // This is the original implementation I came up with on first 'solve'
        public void StringBuilderImplementation()
        {
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
        }
        
        [Benchmark]
        public void StringBuilderWithDeclaredCapacityImplementation()
        {
            for (int i = 1; i <= 100; i++)
            {
                var sb = new StringBuilder(8, 8);

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
        }
        
        
        [Benchmark]
        public void StringImplementation()
        {
            for (int i = 1; i <= 100; i++)
            {
                var s = string.Empty;

                if (i % 3 == 0)
                {
                    s += "Fizz";
                }

                if (i % 5 == 0)
                {
                    s += "Buzz";
                }

                if (string.IsNullOrEmpty(s))
                {
                    s = i.ToString();
                }
    
                Console.WriteLine(s);
            }
        }
        
        [Benchmark]
        public void StringElseIfImplementation()
        {
            for (int i = 1; i <= 100; i++)
            {
                if (i % 3 == 0 && i % 5 == 0)
                {
                    Console.WriteLine("FizzBuzz");
                } else if (i % 3 == 0)
                {
                    Console.WriteLine("Fizz");
                } else if (i % 5 == 0)
                {
                    Console.WriteLine("Buzz");
                }
                else
                {
                    Console.WriteLine(i.ToString());
                }
            }
        }
    }
}