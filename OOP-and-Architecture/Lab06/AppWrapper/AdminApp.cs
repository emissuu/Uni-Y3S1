using Lab6.App.Abstract;
using Lab6.App.Implementations;
using Lab6.App.Models;

namespace Lab6.AppWrapper;

public class AdminApp(List<ICarDealer> carDealers)
{
    public void Run()
    {
        while (true)
        {
            Console.Write($"Choose car dealership(volvo, renault, volkswagen, back): ");
            
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
                case "back":
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
            Console.Write("Enter car's brand: ");
            var inputBrand = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(inputBrand))
            {
                Console.WriteLine("Brand cannot be empty");
                continue;
            }
            Console.Write("Enter car's model: ");
            var inputModel = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(inputModel))
            {
                Console.WriteLine("Model cannot be empty");
                continue;
            }
            
            var car = carDealer.GetCar(inputBrand.ToLower(), inputModel.ToLower());
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
            Console.WriteLine("Enter details of other dealership's car");
            Console.Write("Enter car dealership name: ");
            var inputDealer = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(inputDealer))
            {
                Console.WriteLine("Car dealer cannot be empty");
                continue;
            }
            Console.Write("Enter car's brand: ");
            var inputBrand = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(inputBrand))
            {
                Console.WriteLine("Brand cannot be empty");
                continue;
            }
            Console.Write("Enter car's model: ");
            var inputModel = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(inputModel))
            {
                Console.WriteLine("Model cannot be empty");
                continue;
            }
            
            var dealerInQuestion = carDealers.FirstOrDefault(x => x.DealerName.ToLower() == inputDealer.ToLower());
            if (dealerInQuestion == null)
            {
                Console.WriteLine($"Could not find the dealer.");
                continue;
            }
            var carTheirs = dealerInQuestion.GetCar(inputBrand.ToLower(), inputModel.ToLower());
            if (carTheirs == null)
            {
                Console.WriteLine($"Could not find the car.");
                return;
            }
            
            Console.WriteLine("Enter details of your dealership's car");
            Console.Write("Enter car's brand: ");
            var inputBrandMine = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(inputBrandMine))
            {
                Console.WriteLine("Brand cannot be empty");
                continue;
            }
            Console.Write("Enter car's model: ");
            var inputModelMine = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(inputModelMine))
            {
                Console.WriteLine("Model cannot be empty");
                continue;
            }

            var carYours = carDealer.GetCar(inputBrandMine.ToLower(), inputModelMine.ToLower());
            if (carYours == null)
            {
                Console.WriteLine($"Could not find the car.");
                return;
            }

            var exchanger = new CarExchanger(carDealer, dealerInQuestion);
            var exchangeSuccessful = exchanger.ExchangeCars(carYours, carTheirs);
            if (!exchangeSuccessful)
            {
                Console.WriteLine($"Could not exchange the car.");
                return;
            }
            
            Console.WriteLine("Successfully exchanged car.");

            // if (carDealer.ExchangeCar(dealerInQuestion, carTheirs))
            // {
            //     Console.WriteLine("Successfully exchanged car.");
            // }
            // else
            // {
            //     Console.WriteLine($"Could not exchange the car.");
            // }

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