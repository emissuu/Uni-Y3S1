using Lab6.App.Abstract;
using Lab6.AppWrapper;

namespace Lab6;

public class Program
{
    public static void Main(string[] args)
    {
        List<ICarDealer> carDealers = AppInitializer.CreateDealers();
        Console.WriteLine("Welcome to car dealership app!");
        ChooseAccountType(carDealers);
    }

    private static void ChooseAccountType(List<ICarDealer> carDealers)
    {
        while (true)
        {
            Console.Write("Choose account type(admin, customer): ");
            var input = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            switch (input.Trim())
            {
                case "admin":
                    var adminApp = new AdminApp(carDealers);
                    adminApp.Run();
                    break;
                case "customer":
                    var customerApp = new CustomerApp(carDealers);
                    customerApp.Run();
                    break;
            }
        }
    }
}