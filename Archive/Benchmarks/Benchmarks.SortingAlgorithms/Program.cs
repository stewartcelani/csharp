using Benchmarks.SortingAlgorithms;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Benchmark>();

namespace Benchmarks.SortingAlgorithms
{
    [MemoryDiagnoser()]
    public class Benchmark
    {
        private readonly int[] _array;

        public Benchmark()
        {
            int seededArrayLength = 1000;
            var rand = new Random();
            var tempList = new List<int>();
            for (var i = 1; i <= seededArrayLength; i++)
            {
                int x = rand.Next(1, seededArrayLength);
                tempList.Add(x);
            }
            _array = tempList.ToArray();
        }

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
                    if (current <= next) continue;
                    _array[i] = next;
                    _array[i + 1] = current;
                }
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
                    int current = _array[i];
                    int next = _array[i + 1];
                    if (current <= next) continue;
                    _array[i] = next;
                    _array[i + 1] = current;
                    swapped = true;
                }
                if (!swapped) break; // If already sorted/solved exit early
            }
        }

        [Benchmark]
        public void SelectionSort()
        {
            for (int i = 0; i < _array.Length; i++)
            {
                for (int j = i + 1; j < _array.Length; j++)
                {
                    if (_array[j] < _array[i])
                    {
                        (_array[j], _array[i]) = (_array[i], _array[j]); // swap via destructuring thanks to ReSharper
                    }
                }
            }
        }
        
        [Benchmark]
        public void SelectionSortSwapOnlyOncePerLoop()
        {
            for (var i = 0; i < _array.Length; i++)
            {
                int min = i;
                for (int j = i + 1; j < _array.Length; j++)
                {
                    if (_array[j] < _array[min])
                    {
                        min = j;
                    }
                }
                if (i != min)
                {
                    (_array[i], _array[min]) = (_array[min], _array[i]);
                } 
            }
        }
        
        [Benchmark]
        public void MergeSort()
        {
            int[] sorted = Sort.MergeSort(_array);
        }
    }
}