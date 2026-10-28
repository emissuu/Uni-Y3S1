using Lab12.Interfaces;

namespace Lab12.Services;

public class EmailService : IEmailService
{
    public void SendEmail(string email, string subject, string body)
    {
        // whatever email senders do
    }
}