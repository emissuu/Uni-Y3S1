namespace Lab1;

public abstract class BankAccount
{
    protected readonly int accountNumber;
    protected readonly string AccountOwner;
    protected double Balance = 0;
        
    protected BankAccount(int accountNumber, string accountOwner) =>
        (this.accountNumber, AccountOwner) = (accountNumber, accountOwner);

    public abstract void ShowBalance();

    public abstract void Deposit(double amount);
        
    public abstract void Withdraw(double amount);
}