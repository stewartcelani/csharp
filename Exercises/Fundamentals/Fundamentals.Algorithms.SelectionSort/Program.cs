using BenchmarkDotNet.Attributes;
using Fundamentals.Algorithms.SelectionSort;

var a = new Benchmark();
a.PrintArray();
a.SelectionSort();
a.PrintArray();
Console.WriteLine();

//BenchmarkRunner.Run<Benchmark>();

namespace Fundamentals.Algorithms.SelectionSort
{
    [MemoryDiagnoser]
    public class Benchmark
    {
        private readonly int[] _array;

        public Benchmark()
        {
            var rand = new Random();
            var tempList = new List<int>();
            for (var i = 1; i <= 100; i++)
            {
                var x = rand.Next(1, 100);
                tempList.Add(x);
            }

            _array = tempList.ToArray();
        }

        public void PrintArray()
        {
            Console.WriteLine("[{0}]", string.Join(", ", _array));
        }

        [Benchmark]
        public void SelectionSort()
        {
            for (var i = 0; i < _array.Length; i++)
            for (var j = i + 1; j < _array.Length; j++)
                if (_array[j] < _array[i])
                    (_array[j], _array[i]) = (_array[i], _array[j]); // swap via destructuring thanks to ReSharper
        }

        [Benchmark]
        public void SelectionSortSwapOnlyOncePerLoop()
        {
            for (var i = 0; i < _array.Length; i++)
            {
                var min = i;
                for (var j = i + 1; j < _array.Length; j++)
                    if (_array[j] < _array[min])
                        min = j;
                if (i != min) (_array[i], _array[min]) = (_array[min], _array[i]);
            }
        }
    }
}