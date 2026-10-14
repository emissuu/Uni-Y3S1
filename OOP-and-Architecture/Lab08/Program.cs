namespace Lab8;

public class Program
{
    public static void Main(string[] args)
    {
        var str = "a, 1, 2, f, -1, 0, 4, 10, 4,f, 4f, 8, 9, 3";

        var result = DoMath(str);
        
        Console.WriteLine("Final result: " + result);
    }

    public static int DoMath(string input)
    {
        return input
            .Split(',')
            .Where(x => int.TryParse(x, out _))
            .Select(int.Parse)
            .OrderBy(x => x)
            .Skip(3)
            .Sum();
    }
}