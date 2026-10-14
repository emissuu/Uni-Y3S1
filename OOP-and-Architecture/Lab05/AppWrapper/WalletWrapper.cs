using Lab5.App;
using Lab5.App.Auth;
using Lab5.AppWrapper.Enums;

namespace Lab5.AppWrapper;

public class WalletWrapper
{
    private IDigitalWallet _walletApp;

    public void CreateDigitalWallet()
    {
        AuthProvider chosenAuthProvider = ChooseAuthProvider();
        ILoginProvider loginProvider = CreateLoginProvider(chosenAuthProvider);
        _walletApp = new DigitalWallet(loginProvider);
        Console.WriteLine("Welcome to your digital wallet!");
    }
    
    private AuthProvider ChooseAuthProvider()
    {
        while (true)
        {
            Console.Write("Choose auth provider(gmail, privat24): ");
            var input = Console.ReadLine().ToLower();
            if (input == "privat24")
            {
                return AuthProvider.Privat24;
            }
            else if (input == "gmail")
            {
                return AuthProvider.Gmail;
            }
            else
            {
                continue;
            }
        }
    }

    private ILoginProvider CreateLoginProvider(AuthProvider authProvider)
    {
        while (true)
        {
            try
            {
                Console.Write("Enter your login: ");
                var inputLogin = Console.ReadLine();
                Console.Write("Enter your password: ");
                var inputPassword = Console.ReadLine();

                if (String.IsNullOrWhiteSpace(inputLogin) || String.IsNullOrWhiteSpace(inputPassword))
                {
                    continue;
                }

                Console.WriteLine("Logging in...");
                switch (authProvider)
                {
                    case AuthProvider.Gmail:
                        return new GmailAuthProvider(inputLogin, inputPassword);
                    case AuthProvider.Privat24:
                        return new Privat24AuthProvider(inputLogin, inputPassword);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }

    public void Deposit()
    {
        while (true)
        {
            try {
                Console.Write("Amount: ");
                var inputAmount = Console.ReadLine();
                if (Decimal.TryParse(inputAmount, out decimal amount))
                {
                    _walletApp.Deposit(amount);
                    return;
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Could not process the payment. Error: " + e.Message);
                break;
            }
        }
    }

    public void Withdraw()
    {
        while (true)
        {
            try
            {
                Console.Write("Amount: ");
                var inputAmount = Console.ReadLine();
                if (Decimal.TryParse(inputAmount, out decimal amount))
                {
                    _walletApp.Withdraw(amount);
                    break;
                }

                continue;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Could not process the payment. Error: " + e.Message);
                break;
            }
        }
    }

    public void CheckBalance()
    {
        try
        {
            _walletApp.CheckBalance();
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine("Could not check the balance. Error: " + e.Message);
        }
    }

    public void GetTransactionLog()
    {
        try
        {
            _walletApp.GetTransactionLog();
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine("Could not get the transaction log. Error: " + e.Message);
        }
    }

    public void Login()
    {
        while (true)
        {
            try
            {
                Console.Write("Login: ");
                var inputLogin = Console.ReadLine();
                Console.Write("Password: ");
                var inputPassword = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(inputLogin) || String.IsNullOrWhiteSpace(inputPassword))
                {
                    Console.WriteLine("Invalid login or password");
                    continue;
                }

                _walletApp.Login(inputLogin, inputPassword);
                break;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Could not log in. Error: " + e.Message);
                break;
            }
        }
    }

    public void Logout()
    {
        try
        {
            _walletApp.Logout();
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine("Could not log out. Error: " + e.Message);
        }
    }
}