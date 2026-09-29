using Lab6.App.Abstract;
using Lab6.App.Models;

namespace Lab6.App.Implementations;

public class CarExchanger(ICarDealer carDealer1, ICarDealer carDealer2)
{
    public bool ExchangeCars(Car car1, Car car2)
    {
        bool canExchange1 = carDealer1.CanExchangeCars(car1, car2);
        if (!canExchange1)
        {
            return false;
        }
        bool canExchange2 = carDealer2.CanExchangeCars(car2, car1);
        if (!canExchange2)
        {
            carDealer1.CanExchangeCars(car2, car1);
            return false;
        }
        
        carDealer1.ExchangeCars(car1, car2);
        carDealer2.ExchangeCars(car2, car1);
        return true;
    }
}