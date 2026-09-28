using Lab6.App.Abstract;
using Lab6.App.Models;

namespace Lab6.App.Implementations;

public class Inventory : IInventory
{
    private readonly List<Car> _cars;

    public Inventory(List<Car> cars)
    {
        _cars = cars;
    }
    
    public IReadOnlyList<Car> GetCars()
    {
        return _cars;
    }

    public void AddCar(Car car)
    {
        _cars.Add(car);
    }

    public void RemoveCar(Car car)
    {
        _cars.Remove(car);
    }
}