using Lab12.Domain;

namespace Lab12.Presentation;

public static class InputReader
{
    public static int ReadInt(string prompt)
    {
        while(true)
        {
            var input = ReadString(prompt);

            if (!int.TryParse(input, out int result))
            {
                Console.WriteLine("Input must be a valid number");
                continue;
            }
            
            return result;
        }
    }

    public static string ReadString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty");
                continue;
            }
            
            return input;
        }
    }

    public static TEnum ReadEnum<TEnum>(string prompt) where TEnum : Enum
    {
        while (true)
        {
            Console.WriteLine("Possible options:");
            foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
            {
                Console.WriteLine($"{Convert.ToInt32(value)}. {value}");
            }

            var input = ReadInt(prompt);

            if (!Enum.IsDefined(typeof(TEnum), input))
            {
                Console.WriteLine("There is no such value");
                continue;
            }
            return (TEnum)Enum.ToObject(typeof(TEnum), input);
        }
    }
}