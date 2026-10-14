namespace Lab6.App.Abstract;

public interface IAccount
{
    decimal GetBalance();
    void Deposit(decimal amount);
    bool Withdraw(decimal amount);
}