using Abstract;

var kitty = new Cat();
kitty.MakeNoise(); // Meow.
kitty.Hunt(); // Feed me, human.
kitty.Sleep(); // Zzzzzzzzz...


namespace Abstract
{
    // Abstract classes can only be inherited from
    public abstract class Animal
    {
        /*
         * Abstract methods need to be overriden in inherited class --
         * similar to virtual except virtual methods
         * actually have implementation and can optionally overriden
         */
        public abstract void Hunt();

        public virtual void MakeNoise()
        {
            Console.WriteLine("*generic animal noises*");
        }

        public void Sleep()
        {
            Console.WriteLine("Zzzzzzzzz...");
        }
    }

    public class Cat : Animal
    {
        public override void MakeNoise()
        {
            Console.WriteLine("Meow.");
        }

        public override void Hunt()
        {
            Console.WriteLine("Feed me, human.");
        }
    }
}