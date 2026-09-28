using Lab5.App.Auth;

namespace Lab5.App;

public class DigitalWallet : IDigitalWallet
{
    private decimal _balance = 0;
    private readonly List<string> _transactions = [];

    private ILoginProvider _authProvider;
    
    public DigitalWallet(ILoginProvider authProvider)
    {
        SetAuthProvider(authProvider);
    }
    
    private void SetAuthProvider(ILoginProvider authProvider)
    {
        _authProvider = authProvider;
    } 

    public void Login(string login, string password)
    {
        _authProvider.Validate(login, password);
    }

    public void Logout()
    {
        _authProvider.Logout();
    }
    
    /*
     * Actual balancing, end of auth chapter
     */
    
    public void Deposit(decimal amount)
    {
        _authProvider.EnsureAuthorized();
        _balance += amount;
        _transactions.Add($"[{DateTime.UtcNow.ToString("g")} UTC] Deposited {amount} into wallet.");
        Console.WriteLine($"Successfully deposited {amount} into wallet.");
    }

    public void Withdraw(decimal amount)
    {
        _authProvider.EnsureAuthorized();
        if (_balance >= amount)
        {
            _balance -= amount;
            _transactions.Add($"[{DateTime.UtcNow.ToString("g")}UTC] Withdrawn {amount} from wallet.");
            Console.WriteLine($"Successfully withdrawn {amount} from wallet.");
        }
        else
        {
            _transactions.Add($"[{DateTime.UtcNow.ToString("g")}UTC] Failed to withdraw {amount} from wallet.");
            Console.WriteLine($"Could not withdraw {amount} from wallet. Insufficient funds.");
            
        }
    }

    public void CheckBalance()
    {
        _authProvider.EnsureAuthorized();
        Console.WriteLine($"Wallet Balance: {_balance}");
    }

    public void GetTransactionLog()
    {
        _authProvider.EnsureAuthorized();
        Console.WriteLine($"Transactions log:");
        foreach (var transaction in _transactions)
        {
            Console.WriteLine(transaction);
        }
    }
}