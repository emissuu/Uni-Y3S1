namespace Lab2;

class Snake : Animal
{
    public override void Sound()
    {
        Console.WriteLine("ssssss ssssssssssss");
    }

    public override void Walk()
    {
        Console.WriteLine("*crawls*");
    }
}