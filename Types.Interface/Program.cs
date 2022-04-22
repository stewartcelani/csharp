IDog japanezeSpitz = new JapanezeSpitz();
japanezeSpitz.Bark(); // Woof woof!

IDog bulldog = new Bulldog();
bulldog.Bark(); // Grrrrrrr!

interface IDog
{
    public void Bark();
}

public class JapanezeSpitz : IDog
{
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