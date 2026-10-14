namespace Lab1;

public class CurrentAccount : BankAccount
{
    private double _creditLimit = 0;

    public CurrentAccount(int accountNumber, string accountOwner)
        : base(accountNumber, accountOwner) { }

    public override void ShowBalance()
    {
        Console.WriteLine($"Account {accountNumber} balance: {Math.Floor((Balance + _creditLimit) * 100) / 100}");
    }

    public override void Deposit(double amount)
    {
        Balance += amount;
        Console.WriteLine($"Deposited {amount} into account number {accountNumber}");
    }

    public override void Withdraw(double amount)
    {
        if (Balance + _creditLimit >= amount)
        {
            Balance -= amount;
            Console.WriteLine($"Withdrawn {amount} from account number {accountNumber}");
        }
        else
        {
            Console.WriteLine($"Couldn't withdraw {amount} from account number {accountNumber}");
        }
    }

    public void ChangeCreditLimit(double amount)
    {
        _creditLimit += amount;
        Console.WriteLine($"Account {accountNumber} credit limit changed to {amount}");
    }
}