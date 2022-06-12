using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Fundamentals.Algorithms.BubbleSort;

/*
var a = new Benchmark();
a.PrintArray();
a.BubbleSortImproved();
a.PrintArray();
Console.WriteLine();

var b = new Benchmark();
b.PrintArray();
b.BubbleSortImproved();
b.PrintArray();
Console.WriteLine();
*/

BenchmarkRunner.Run<Benchmark>();

namespace Fundamentals.Algorithms.BubbleSort
{
    [MemoryDiagnoser]
    public class Benchmark
    {
        private readonly int[] _array;
        private readonly int[] _arrayDesc;

        public Benchmark()
        {
            var rand = new Random();
            var tempList = new List<int>();
            var tempDescList = new List<int>();
            for (var i = 1; i <= 1000; i++)
            {
                var x = rand.Next(1, 1001);
                tempList.Add(x);
            }

            for (var i = 1000; i >= 1; i--) tempDescList.Add(i);
            _array = tempList.ToArray();
            _arrayDesc = tempDescList.ToArray();
        }

        public void PrintArray()
        {
            Console.WriteLine("[{0}]", string.Join(", ", _array));
        }

        [Benchmark]
        public void BubbleSort()
        {
            for (var j = 0; j < _array.Length - 1; j++)
            for (var i = 0; i < _array.Length - 1; i++)
            {
                var current = _array[i];
                var next = _array[i + 1];
                if (current <= next) continue;
                _array[i] = next;
                _array[i + 1] = current;
            }
        }

        [Benchmark]
        public void BubbleSortWorstCase()
        {
            for (var j = 0; j < _arrayDesc.Length - 1; j++)
            for (var i = 0; i < _arrayDesc.Length - 1; i++)
            {
                var current = _arrayDesc[i];
                var next = _arrayDesc[i + 1];
                if (current <= next) continue;
                _arrayDesc[i] = next;
                _arrayDesc[i + 1] = current;
            }
        }

        [Benchmark]
        public void BubbleSortImprovedWorstCase()
        {
            while (true)
            {
                var swapped = false;
                for (var i = 0; i < _arrayDesc.Length - 1; i++)
                {
                    var current = _arrayDesc[i];
                    var next = _arrayDesc[i + 1];
                    if (current <= next) continue;
                    _arrayDesc[i] = next;
                    _arrayDesc[i + 1] = current;
                    swapped = true;
                }

                if (!swapped) break; // If already sorted/solved exit early
            }
        }

        [Benchmark]
        public void BubbleSortImproved()
        {
            while (true)
            {
                var swapped = false;
                for (var i = 0; i < _array.Length - 1; i++)
                {
                    var current = _array[i];
                    var next = _array[i + 1];
                    if (current <= next) continue;
                    _array[i] = next;
                    _array[i + 1] = current;
                    swapped = true;
                }

                if (!swapped) break; // If already sorted/solved exit early
            }
        }
    }
}