namespace Lab1;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("===== Deposit Account =====");
        
        var depositAccount = new DepositAccount(1, "SuperDanya2017", 0.5);
        
        depositAccount.ShowBalance();
        depositAccount.Deposit(50);
        depositAccount.Withdraw(60);
        depositAccount.EnhanceBalance();
        depositAccount.ShowBalance();
        depositAccount.Withdraw(60);
        depositAccount.ShowBalance();
        
        
        Console.WriteLine("\n===== Current Account =====");
        var currentAccount = new CurrentAccount(2, "AlexBosss_");
        
        currentAccount.ShowBalance();
        currentAccount.Deposit(200);
        currentAccount.Withdraw(400);
        currentAccount.ChangeCreditLimit(500);
        currentAccount.ShowBalance();
        currentAccount.Withdraw(400);
        currentAccount.ShowBalance();
    }
}