using Fundamentals.Interface;

var japanezeSpitz = new JapanezeSpitz();
japanezeSpitz.Name = "Lynx";
japanezeSpitz.Bark(); // Woof woof!
japanezeSpitz.Sleep(); // Zzz...

var bulldog = new Bulldog();
bulldog.Bark(); // Grrrrrrr!

namespace Fundamentals.Interface
{
    interface IDog
    {
        void Bark();
    }

    interface IAnimal
    {
        void Sleep();
    }

    public class JapanezeSpitz : IDog, IAnimal
    {
        public string Name { get; set; }

        public void Bark()
        {
            Console.WriteLine("Woof woof!");
        }

        public void Sleep()
        {
            Console.WriteLine("Zzz...");
        }
    }

    public class Bulldog : IDog
    {
        public void Bark()
        {
            Console.WriteLine("Grrrrrrr!");
        }
    }
}