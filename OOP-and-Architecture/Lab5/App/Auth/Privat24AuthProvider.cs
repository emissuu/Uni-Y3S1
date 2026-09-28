using Lab5.App.Common;

namespace Lab5.App.Auth;

public class Privat24AuthProvider : ILoginProvider
{
    private string _phoneNumber;
    private string _password;
    
    private bool _isAuthorized;
    
    public Privat24AuthProvider(string login, string password)
    {
        _phoneNumber = login;
        _password = PasswordHasher.HashPassword(password);   
        _isAuthorized = true;
        Console.WriteLine("Successfully logged in with Privat24");
    }
    
    public void EnsureAuthorized()
    {
        if (!_isAuthorized)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }
    }

    public void Validate(string login, string password)
    {
        if (_isAuthorized)
        {
            Console.WriteLine("Already authorized with Privat24");
            return;
        }
        if (_phoneNumber == login && PasswordHasher.VerifyHashedPassword(_password, password))
        {
            _isAuthorized = true;
            Console.WriteLine("Successfully logged in with Privat24");
            return;
        }
        throw new UnauthorizedAccessException("Invalid login or password");
    }

    public void Logout()
    {
        EnsureAuthorized();
        Console.WriteLine("Successfully logged out");
        _isAuthorized = false;
    }
}