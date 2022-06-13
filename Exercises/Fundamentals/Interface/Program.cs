using Interface;

var japanezeSpitz = new JapanezeSpitz();
japanezeSpitz.Name = "Lynx";
japanezeSpitz.Bark(); // Woof woof!
japanezeSpitz.Sleep(); // Zzz...

var bulldog = new Bulldog();
bulldog.Bark(); // Grrrrrrr!

namespace Interface
{
    internal interface IDog
    {
        void Bark();
    }

    internal interface IAnimal
    {
        void Sleep();
    }

    public class JapanezeSpitz : IDog, IAnimal
    {
        public string Name { get; set; }

        public void Sleep()
        {
            Console.WriteLine("Zzz...");
        }

        public void Bark()
        {
            Console.WriteLine("Woof woof!");
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