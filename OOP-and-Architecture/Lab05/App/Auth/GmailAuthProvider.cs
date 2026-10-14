using Lab5.App.Common;

namespace Lab5.App.Auth;

public class GmailAuthProvider : ILoginProvider
{
    private string _email;
    private string _password;
    
    private bool _isAuthorized;
    
    public GmailAuthProvider(string login, string password)
    {
        _email = login;
        _password = PasswordHasher.HashPassword(password);
        _isAuthorized = true;
        Console.WriteLine("Successfully logged in with Gmail");
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
            Console.WriteLine("Already authorized with Gmail");
            return;
        }
        if (_email == login && PasswordHasher.VerifyHashedPassword(_password, password))
        {
            _isAuthorized = true;
            Console.WriteLine("Successfully logged in with Gmail");
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