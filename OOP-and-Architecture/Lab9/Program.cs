namespace Lab9;

class Program
{
    static void Main(string[] args)
    {
        var array = new int[] {2, 54, 19, 0, 23, -4, 85, 92, 9, -56};
        
        Console.WriteLine("Starting array: ");
        Console.WriteLine(string.Join(", ", array));

        Console.WriteLine("Sorted by ascending: ");
        Console.WriteLine(string.Join(", ", BubbleSort.Sort(array, (a, b) => a > b)));
        
        Console.WriteLine("Sorted by descending: ");
        Console.WriteLine(string.Join(", ", BubbleSort.Sort(array, (a, b) => a < b)));
    }
}