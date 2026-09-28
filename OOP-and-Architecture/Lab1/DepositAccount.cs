namespace Lab1;

public class DepositAccount : BankAccount
{
    private double _depositRate;
        
    public DepositAccount(int accountNumber, string accountOwner, double depositRate = 0.1)
        : base(accountNumber, accountOwner)
    {
        _depositRate = depositRate;
    }

    public override void ShowBalance()
    {
        Console.WriteLine($"Account {accountNumber} balance: {Math.Floor(Balance * 100) / 100}");
    }

    public override void Deposit(double amount)
    {
        Balance += amount;
        Console.WriteLine($"Deposited {amount} into account {accountNumber}");
    }

    public override void Withdraw(double amount)
    {
        if (Balance >= amount)
        {
            Balance -= amount;
            Console.WriteLine($"Withdrawn {amount} from account {accountNumber}");
        }
        else
        {
            Console.WriteLine($"Couldn't withdraw {amount} from account {accountNumber}");
        }
    }

    public void EnhanceBalance()
    {
        Balance *= _depositRate + 1;
        Console.WriteLine($"Balanced enhanced by {(_depositRate + 1) * 100}% in account {accountNumber}");
    }
}