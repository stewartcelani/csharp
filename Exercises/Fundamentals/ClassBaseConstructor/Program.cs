using ClassBaseConstructor;

var bob = new Male("Bob", 22);
Console.WriteLine($"Name: {bob.Name}, Age: {bob.Age}, Gender: {bob.Gender}");
// Name: Bob, Age: 22, Gender: Male

namespace ClassBaseConstructor
{
    public abstract class Person
    {
        protected Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public int Age { get; set; }
        public string Name { get; set; }
    }

    public class Male : Person
    {
        public Male(string name, int age) : base(name, age)
        {
            Gender = Gender.Male;
        }

        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}