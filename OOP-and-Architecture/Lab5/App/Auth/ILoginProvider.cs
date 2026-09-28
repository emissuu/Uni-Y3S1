namespace Lab5.App.Auth;

public interface ILoginProvider
{
    public void EnsureAuthorized();
    public void Validate(string login, string password);
    public void Logout();
}