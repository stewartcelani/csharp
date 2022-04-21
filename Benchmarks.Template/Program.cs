using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Bogus;

namespace Benchmarks.Template
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmark>();
        }
    }

    [MemoryDiagnoser()]
    public class Benchmark
    {
        public Benchmark()
        {
            // Any Setup
            SetupForBenchmark();
        }

        private void SetupForBenchmark()
        {
           var faker = new Faker();

        }
        
        [Benchmark]
        public void Bechmark1()
        {
            
        }

        [Benchmark]
        public void Bechmark2()
        {
            
        }
    }
    
}