namespace Lab11;

class Program
{
    static void Main(string[] args)
    {
        var calculator = new Calculator<float>();

        Console.WriteLine(calculator.Divide(2, 0));
    }
}