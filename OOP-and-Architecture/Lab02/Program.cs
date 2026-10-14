namespace Lab2;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("===== Snake =====");
        Animal snake = new Snake();
        snake.Walk();
        snake.Sound();
        
        Console.WriteLine("\n===== Cat =====");
        Animal cat = new Cat();
        cat.Walk();
        cat.Sound();
        
        Console.WriteLine("\n===== Dog =====");
        Animal dog = new Dog();
        dog.Walk();
        dog.Sound();
    }
}