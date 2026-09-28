using Lab6.App.Abstract;
using Lab6.App.Models;

namespace Lab6.AppWrapper;

public class AdminApp(List<ICarDealer> carDealers)
{
    public void Run()
    {
        while (true)
        {
            Console.Write($"Choose car dealership(volvo, renault, volkswagen, exit): ");
            
            var input = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            switch (input.ToLower().Trim())
            {
                case "volvo": 
                    DealerMenu(carDealers.First(x => x.DealerName.ToLower() == "volvo"));
                    break;
                case "renault":
                    DealerMenu(carDealers.First(x => x.DealerName.ToLower() == "renault"));
                    break;
                case "volkswagen":
                    DealerMenu(carDealers.First(x => x.DealerName.ToLower() == "volkswagen"));
                    break;
                case "exit":
                    return;
            }
        }
    }

    private void DealerMenu(ICarDealer carDealer)
    {
        while (true)
        {
            Console.Write("Choose your action:\n" +
                         "1 - List cars\n" +
                         "2 - List all cars from other dealerships\n" +
                         "3 - Sell car\n" +
                         "4 - Exchange car with other dealership\n" +
                         "5 - Check balance\n" +
                         "0 - Back\n" +
                         "Action: ");
            var input = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(input))
            {
                continue;
            }
            switch (input.Trim())
            {
                case "1":
                    Console.WriteLine("All cars at car dealership:");
                    ListCars(carDealer.GetAllCarsAdmin().ToList());
                    break;
                case "2":
                    Console.WriteLine("All cars at other car dealerships:");
                    ListCars(carDealers
                        .Where(x => x.DealerName != carDealer.DealerName)
                        .SelectMany(x => x.GetAllCarsAdmin())
                        .ToList());
                    break;
                case "3":
                    SellCar(carDealer);
                    break;
                case "4":
                    ExchangeCar(carDealer);
                    break;
                case "5":
                    Console.WriteLine($"Balance: {carDealer.GetBalance()}");
                    break;
                case "0":
                    return;
            }
        }
    }

    private void SellCar(ICarDealer carDealer)
    {
        while (true)
        {
            Console.Write("Enter car's model: ");
            var inputModel = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(inputModel))
            {
                continue;
            }
            
            var car = carDealer.GetCar(carDealer.DealerName, inputModel);
            if (car == null)
            {
                Console.WriteLine($"Could not find the car.");
                return;
            }

            if (carDealer.SellCar(car))
            {
                Console.WriteLine("Successfully sold car.");
            }
            else
            {
                Console.WriteLine($"Could not sell car.");
            }

            return;
        }
    }

    private void ExchangeCar(ICarDealer carDealer)
    {
        while (true)
        {
            Console.Write("Enter car's brand: ");
            var inputBrand = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(inputBrand))
            {
                continue;
            }
            Console.Write("Enter car's model: ");
            var inputModel = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(inputModel))
            {
                continue;
            }

            var dealerInQuestion = carDealers.First(x => x.DealerName.ToLower() == inputBrand.ToLower());
            var car = dealerInQuestion.GetCar(inputBrand.ToLower(), inputModel.ToLower());
            if (car == null)
            {
                Console.WriteLine($"Could not find the car.");
                return;
            }

            if (carDealer.ExchangeCar(dealerInQuestion, car))
            {
                Console.WriteLine("Successfully exchanged car.");
            }
            else
            {
                Console.WriteLine($"Could not exchange the car.");
            }

            return;
        }
    }

    private void ListCars(List<Car> cars)
    {
        for (int i = 0; i < cars.Count; i++)
        {
            Console.WriteLine($"{i}. {cars[i].Brand} {cars[i].Model} - {cars[i].Year}. ${cars[i].Price}");
        }
    }
}