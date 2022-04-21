using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Bogus;

namespace Template
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchy>();
        }
    }

    [MemoryDiagnoser()]
    public class Benchy
    {
        public Benchy()
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