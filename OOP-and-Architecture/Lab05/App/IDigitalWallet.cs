namespace Lab5.App;

public interface IDigitalWallet
{
    public void Deposit(decimal amount);
    public void Withdraw(decimal amount);
    public void CheckBalance();
    public void GetTransactionLog();
    
    public void Login(string login, string password);
    public void Logout();
}