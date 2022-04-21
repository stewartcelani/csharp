using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Bogus;

namespace Dictionary // Note: actual namespace depends on the project name.
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
        ConcurrentDictionary<Guid, Person> PersonDictionary = new ConcurrentDictionary<Guid, Person>();

        public Benchy()
        {
                SeedDictionary();
        }

        private void SeedDictionary()
        {
            var faker = new Faker("en");
            for (int i = 0; i < 1000; i++)
            {
                PersonDictionary[Guid.NewGuid()] = new Person(faker.Name.FullName(), faker.Random.Number(18, 67));
            }
        }
        
        [Benchmark]
        public void ValuesLoop()
        {
            foreach (Person p in PersonDictionary.Values)
            {
                p.Age++;
            }
        }

        [Benchmark]
        public void KeysLoop()
        {
            foreach (Guid iGuid in PersonDictionary.Keys)
            {
                PersonDictionary[iGuid].Age++;
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