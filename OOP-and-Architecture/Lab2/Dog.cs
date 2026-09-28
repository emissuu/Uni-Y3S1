namespace Lab2;

class Dog : Animal
{
    public override void Sound()
    {
        Console.WriteLine("arf, wruff");
    }

    public override void Walk()
    {
        Console.WriteLine("*walks fast*");
    }
}