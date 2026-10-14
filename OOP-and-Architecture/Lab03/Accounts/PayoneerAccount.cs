namespace Lab3.Accounts;

class PayoneerAccount : Account
{
    public override void ProcessPayment(int paymentNumber)
    {
        Console.WriteLine($"Payment {paymentNumber} at Payoneer processed successfully!");
    }
}