using Types.Class.BaseConstructor;

var bob = new Male("Bob", 22);
Console.WriteLine($"Name: {bob.Name}, Age: {bob.Age}, Gender: {bob.Gender}");
// Name: Bob, Age: 22, Gender: Male

namespace Types.Class.BaseConstructor
{
    public abstract class Person
    {
        public int Age { get; set; }
        public string Name { get; set; }

        protected Person(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }
    }

    public class Male : Person
    {
        public Gender Gender { get; set; }

        public Male(string name, int age) : base(name, age)
        {
            this.Gender = Gender.Male;
        }
    }

    public enum Gender
    {
        Male,
        Female
    }
}