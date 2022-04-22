var fruit = new Fruit();
fruit.PrintName(); // Fruit

var orange = new Orange();
orange.PrintName(); // Orange

var lemon = new Lemon();
lemon.PrintName(); // Fruit


public class Fruit
{
    public virtual void PrintName()
    {
        Console.WriteLine("Fruit");
    }
}

public class Orange : Fruit
{
    public override void PrintName()
    {
        Console.WriteLine("Orange");
    }
}

public class Lemon : Fruit
{
    
}
