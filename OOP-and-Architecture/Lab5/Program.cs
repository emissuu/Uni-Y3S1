using Lab5.AppWrapper;

namespace Lab5;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Digital Wallet!");
        
        var walletApp = new WalletWrapper();
        walletApp.CreateDigitalWallet();

        bool loop = true;
        while (loop)
        {
            Console.Write("Choose your action:\n" +
                              "1 - Deposit\n" +
                              "2 - Withdraw\n" +
                              "3 - Check balance\n" +
                              "4 - Get transaction log\n" +
                              "5 - Log in\n" +
                              "6 - Log out\n" +
                              "0 - Exit\n" +
                              "Choose an option: ");
            var input = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            switch (input[0])
            {
                case '1':
                    walletApp.Deposit();
                    break;
                case '2':
                    walletApp.Withdraw();
                    break;
                case '3':
                    walletApp.CheckBalance();
                    break;
                case '4':
                    walletApp.GetTransactionLog();
                    break;
                case '5':
                    walletApp.Login();
                    break;
                case '6':
                    walletApp.Logout();
                    break;
                case '0':
                    loop = false;
                    Console.WriteLine("See ya!");
                    break;
            }
        }
    }
}