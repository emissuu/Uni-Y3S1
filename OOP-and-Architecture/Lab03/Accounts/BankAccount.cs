namespace Lab3.Accounts;

class BankAccount : Account
{
    public override void ProcessPayment(int paymentNumber)
    {
        Console.WriteLine($"Payment {paymentNumber} at Bank processed successfully!");
    }
}