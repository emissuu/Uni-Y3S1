using Lab6.App.Models;

namespace Lab6.App.Abstract;

public interface ICarDealer
{
    string DealerName { get; }
    IReadOnlyList<Car> GetAllCarsCustomer();
    IReadOnlyList<Car> GetAllCarsAdmin();
    Car? GetCar(string brand, string model);
    decimal GetBalance();
    bool BuyCarCustomer(Car car);
    bool BuyCarDealer(Car car);
    bool CanExchangeCars(Car car1, Car car2);
    void ExchangeCars(Car car1, Car car2);
    bool SellCar(Car car);
    bool ExchangeCar(ICarDealer otherCarDealer, Car car);
}