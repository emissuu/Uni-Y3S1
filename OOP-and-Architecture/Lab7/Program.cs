using System.Collections;

namespace Lab7;

public class Program 
{
    public static void Main(string[] args)
    {
        var linkedList = new UserLinkedList<int>(2);
        linkedList.AddLast(2);
        linkedList.AddFirst(1);
        PrintList(linkedList);
        
        // 2
        var result1 = linkedList.FindLast(2);
        if (result1 is null)
            Console.WriteLine("No element found");
        else
            Console.WriteLine("Found element: " + result1.Data);

        // 3
        linkedList.RemoveLast();
        linkedList.RemoveFirst();
        PrintList(linkedList);
    }

    public static void PrintList(IEnumerable list)
    {
        foreach (var item in list)
        {
            Console.Write(item.ToString() + ' ');
        }
        Console.WriteLine();
    }
}

