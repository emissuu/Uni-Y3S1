using Lab6.App.Abstract;
using Lab6.App.Implementations;
using Lab6.App.Models;

namespace Lab6;

public static class AppInitializer
{
    public static List<ICarDealer> CreateDealers()
    {
        
        List<ICarDealer> carDealers = [];
        carDealers.Add(new CarDealer(
            "Volvo",
            new Inventory(
            new List<Car>([
                    new Car("Volvo", "XC60", 2021, 32_500),
                    new Car("Volvo", "S90", 2019, 28_000),
                    new Car("Volvo", "V40", 2018, 18_500)]
                )
            ),
            new CurrentAccount(43_000)));
        
        carDealers.Add(new CarDealer(
            "Renault",
            new Inventory(
                new List<Car>([
                        new Car("Renault", "Megane", 2020, 17_500),
                        new Car("Renault", "Clio", 2022, 15_000),
                        new Car("Renault", "Captur", 2021, 21_000),
                        new Car("Renault", "Talisman", 2019, 19_500)]
                )
            ),
            new CurrentAccount(31_000)));
        carDealers.Add(new CarDealer(
            "Volkswagen",
            new Inventory(
                new List<Car>([
                        new Car("Volkswagen", "Golf", 2020, 20_000),
                        new Car("Volkswagen", "Passat", 2018, 18_500),
                        new Car("Volkswagen", "Tiguan", 2022, 27_000),]
                )
            ),
            new CurrentAccount(34_000)));
        
        return carDealers;
    }
}