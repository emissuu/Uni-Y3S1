namespace Lab4;

class Program
{
    static void Main(string[] args)
    {
        var coffeeMachine1Pro = new CoffeeMachine(60, 2200);
        ICoffeeMachine coffeeMachine = coffeeMachine1Pro;
        
        Console.WriteLine(
            "Heeeyyyy! Welcome to the new wireless smart and modern coffee machine. \n" +
            "Why are you here?");
        while (true)
        {
            Console.Write(
              "s - Just checking state\n" +
              "e - One espresso please\n" +
              "l - One latte please\n" +
              "exit - Already leaving!\n" +
              "Input: ");
            
            var input  = Console.ReadLine();
            Console.WriteLine();
            if (!input.IsWhiteSpace() && input is not null)
                switch (input.ToLower())
                {
                    case "s":
                    case "state":
                        Console.WriteLine("Coffee machine is well!\n" +
                                          $"Amount of water: {coffeeMachine.HowMuchWater}ml\n" +
                                          $"Amount of beans: {coffeeMachine.HowMuchCoffeeBeans}g");
                        Console.WriteLine("\nAnything else?");
                        break;
                    case "e":
                    case "espresso":
                        coffeeMachine.MakeEspresso();
                        Console.WriteLine("\nAnything else?");
                        break;
                    case "l":
                    case "latte":
                        coffeeMachine.MakeLatte();
                        Console.WriteLine("\nAnything else?");
                        break;
                    case "exit":
                        Console.WriteLine("See ya!\nLeaving...");
                        return;
                    default:
                        Console.WriteLine("What? What did you say?");
                        break;
                }
        }
    }
}

