using Lab6.App.Models;

namespace Lab6.App.Abstract;

public interface IInventory
{
    IReadOnlyList<Car> GetCars();
    void AddCar(Car car);
    void RemoveCar(Car car);
}