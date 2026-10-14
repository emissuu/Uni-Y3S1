namespace Lab2;

class Cat : Animal
{
    public override void Sound()
    {
        Console.WriteLine("meow, mrow");
    }

    public override void Walk()
    {
        Console.WriteLine("*walks lazily*");
    }
}