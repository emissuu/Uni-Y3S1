using Lab3.Accounts;

namespace Lab3;

class Program
{
    static void Main(string[] args)
    {
        var paymentProcessor = new PaymentProcessor();
        
        Console.WriteLine("===== BankAccount =====");
        var bankAccount = new BankAccount();
        paymentProcessor.ProcessPayment(bankAccount);
        
        Console.WriteLine("\n===== PayoneerAccount =====");
        var payoneerAccount = new PayoneerAccount();
        paymentProcessor.ProcessPayment(payoneerAccount);
        
        Console.WriteLine("\n===== WiseAccount =====");
        var wiseAccount = new WiseAccount();
        paymentProcessor.ProcessPayment(wiseAccount);
    }
}