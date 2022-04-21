using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Bogus;

namespace Dictionary
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
        private readonly ConcurrentDictionary<Guid, Person> _personDictionary = new ConcurrentDictionary<Guid, Person>();

        public Benchy()
        {
                SeedDictionary();
        }

        private void SeedDictionary()
        {
            var faker = new Faker("en");
            for (int i = 0; i < 1000; i++)
            {
                _personDictionary[Guid.NewGuid()] = new Person(faker.Name.FullName(), faker.Random.Number(18, 67));
            }
        }
        
        [Benchmark]
        public void ValuesLoop()
        {
            foreach (Person p in _personDictionary.Values)
            {
                p.Age++;
            }
        }

        [Benchmark]
        public void KeysLoop()
        {
            foreach (Guid g in _personDictionary.Keys)
            {
                _personDictionary[g].Age++;
            }
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Person(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }
    }
}