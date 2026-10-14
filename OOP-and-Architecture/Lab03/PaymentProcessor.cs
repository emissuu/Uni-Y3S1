using Lab3.Accounts;

namespace Lab3;

class PaymentProcessor
{
    public void ProcessPayment(Account account)
    {
        int paymentNumber = new Random().Next(1, 1000);
            
        account.ProcessPayment(paymentNumber);
    }

    public void ProcessPayment(BankAccount account)
    {
        int paymentNumber = new Random().Next(1, 1000);
            
        account.ProcessPayment(paymentNumber);
    }
}