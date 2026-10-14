using Lab6.App.Abstract;
using Lab6.App.Models;

namespace Lab6.AppWrapper;

public class CustomerApp(List<ICarDealer> carDealers)
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
                    CustomerMenu(carDealers.First(x => x.DealerName.ToLower() == "volvo"));
                    break;
                case "renault":
                    CustomerMenu(carDealers.First(x => x.DealerName.ToLower() == "renault"));
                    break;
                case "volkswagen":
                    CustomerMenu(carDealers.First(x => x.DealerName.ToLower() == "volkswagen"));
                    break;
                case "back":
                    return;
            }
        }
    }

    private void CustomerMenu(ICarDealer carDealer)
    {
        while (true)
        {
            Console.Write("Choose your action:\n" +
                          "1 - List cars\n" +
                          "2 - Buy car\n" +
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
                    ListCars(carDealer.GetAllCarsCustomer().ToList());
                    break;
                case "2":
                    BuyCar(carDealer);
                    break;
                case "0":
                    return;
                    
            }
        }
    }

    private void BuyCar(ICarDealer carDealer)
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
                Console.WriteLine("Couldn't find car.");
                return;
            }

            if (carDealer.BuyCarCustomer(car))
            {
                Console.WriteLine($"{car.Brand} {car.Model} has been successfully bought.");
            }
            else
            {
                Console.WriteLine($"{car.Brand} {car.Model} has been failed to buy.");
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