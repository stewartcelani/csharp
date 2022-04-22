using BenchmarkDotNet;
using Algorithms.BubbleSort;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

//BenchmarkRunner.Run<Benchmark>();

var b = new Benchmark();
b.PrintArray();
b.BubbleSort();
b.PrintArray();

/*
var c = new Benchmark();
c.PrintArray();
c.LinqOrderBy();
c.PrintArray();
*/

namespace Algorithms.BubbleSort
{
    [MemoryDiagnoser()]
    public class Benchmark
    {
        private int[] _array = new int[] { 5, 232, 3, 1231, 2, 100, 1312, 6, 23, 21, 15, 9, 29321, 1, 2 };

        public void PrintArray()
        {
            Console.WriteLine("[{0}]", string.Join(", ", _array));
        }
        
        [Benchmark]
        public void BubbleSort()
        {
            for (var j = 0; j < _array.Length - 1; j++)
            {
                for (var i = 0; i < _array.Length - 1; i++)
                {
                    int current = _array[i];
                    int next = _array[i + 1];
                    if (current > next)
                    {
                        _array[i] = next;
                        _array[i + 1] = current;
                    }
                }
            }
        }

        [Benchmark]
        public void LinqOrderBy()
        {
            _array = _array.OrderBy(i => i).ToArray();
        }
    }
}