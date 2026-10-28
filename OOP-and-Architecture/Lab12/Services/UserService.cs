using Lab12.Domain;
using Lab12.Interfaces;

namespace Lab12.Services;

public class UserService : IUserService
{
    private readonly List<User> _users = [];

    public IReadOnlyList<User> GetAll()
    {
        return _users;
    }

    public User? GetById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public void Add(string login, string email)
    {
        int newId = _users.Count + 1;
        _users.Add(new User(newId, login, email));
    }

    public bool Update(int userId, string login, string email)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return false;
        
        user.Update(login, email);
        return true;
    }

    public bool Delete(int userId)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return false;
        
        _users.Remove(user);
        return true;
    }

    public bool Subscribe(int userId, BookCategory bookCategory)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return false;
        
        user.Subscribe(bookCategory);
        return true;
    }

    public bool Unsubscribe(int userId, BookCategory bookCategory)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return false;
        
        user.Unsubscribe(bookCategory);
        return true;
    }
}