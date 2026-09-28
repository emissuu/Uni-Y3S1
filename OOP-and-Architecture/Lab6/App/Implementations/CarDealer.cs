using System.Globalization;
using Lab6.App.Abstract;
using Lab6.App.Models;

namespace Lab6.App.Implementations;

public class CarDealer : ICarDealer
{
    private string _dealerName;
    private readonly IInventory _inventory;
    private readonly IAccount _account;
    private readonly decimal _customerPriceMultiplier = 1.15m;
    
    public CarDealer(string dealerName, IInventory inventory, IAccount account)
    {
        _dealerName = dealerName;
        _inventory = inventory;
        _account = account;
    }
    
    public string DealerName => _dealerName;

    public IReadOnlyList<Car> GetAllCarsCustomer()
    {
        var cars = _inventory.GetCars();
        return cars.Select(x => new Car(
            x.Brand,
            x.Model,
            x.Year,
            x.Price * _customerPriceMultiplier
        )).ToList();
    }
    
    public IReadOnlyList<Car> GetAllCarsAdmin()
    {
        return _inventory.GetCars();
    }

    public Car? GetCar(string brand, string model)
    {
        return _inventory.GetCars().FirstOrDefault(x => x.Brand.ToLower() == brand && x.Model.ToLower() == model);
    }

    public decimal GetBalance() => _account.GetBalance();
    
    public bool BuyCarCustomer(Car car)
    {
        return BuyCar(car, car.Price * _customerPriceMultiplier);
    }

    public bool BuyCarDealer(Car car)
    {
        return BuyCar(car, car.Price);
    }

    public bool SellCar(Car car)
    {
        if (_account.Withdraw(car.Price))
        {
            _inventory.AddCar(car);
            return true;
        }

        return false;
    }

    public bool ExchangeCar(ICarDealer otherCarDealer, Car car)
    {
        if (_account.Withdraw(car.Price))
        {
            if (otherCarDealer.BuyCarDealer(car))
            {
                _inventory.AddCar(car);
                return true;
            }
            else
            {
                _account.Deposit(car.Price);
            }
        }
        return false;
    }

    private bool BuyCar(Car car, decimal price)
    {
        var existingCar = _inventory.GetCars().FirstOrDefault(x => x == car);
        if (existingCar == null)
        {
            return false;
        }
        _account.Deposit(price);
        _inventory.RemoveCar(existingCar);
        
        return true;
    }
}