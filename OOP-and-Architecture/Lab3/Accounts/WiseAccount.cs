namespace Lab3.Accounts;

class WiseAccount : Account
{
    public override void ProcessPayment(int paymentNumber)
    {
        Console.WriteLine($"Payment {paymentNumber} at Wise processed successfully!");
    }
}