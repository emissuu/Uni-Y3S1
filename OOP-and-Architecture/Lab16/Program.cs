using Lab16.App;

namespace Lab16;

public class Program
{
    public static void Main()
    {
        var textEditor = new TextEditor();
        var caretaker = new Caretaker(textEditor);
        
        Console.WriteLine("Hello! Welcome to the text editor!");
        while (true)
        {
            Console.WriteLine($"Current value: {textEditor.GetText()}");

            Console.WriteLine("Choose an option:\n" +
                              "1 - Edit value\n" +
                              "2 - Undo\n" +
                              "3 - See log");

            var input = ReadInt("Choice: ");

            switch (input)
            {
                case 1:
                    Console.Write("Enter new value: ");
                    var text = Console.ReadLine() ?? "";

                    caretaker.MakeBackup();
                    textEditor.SetText(text);
                    break;
                case 2:
                    caretaker.Undo();
                    break;
                case 3:
                    Console.WriteLine("Backup log:");
                    foreach (var log in caretaker.GetBackupLog().OrderBy(x => x.GetTime()))
                    {
                        Console.WriteLine($"{log.GetTime().ToLongTimeString()}: {log.GetText()}");
                    }
                    break;
                default:
                    Console.WriteLine("Option does not exist");
                    break;
            }
            Console.WriteLine();
        }
    }
    
    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(input) || !int.TryParse(input, out _))
            {
                Console.WriteLine("Invalid input");
                continue;
            }
            
            return int.Parse(input);
        }
    }
}