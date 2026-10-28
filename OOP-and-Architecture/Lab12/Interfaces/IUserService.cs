using Lab12.Domain;

namespace Lab12.Interfaces;

public interface IUserService
{
    IReadOnlyList<User> GetAll();
    User? GetById(int id);
    void Add(string login, string email);
    bool Update(int userId, string login, string email);
    bool Delete(int userId);
    bool Subscribe(int userId, BookCategory bookCategory);
    bool Unsubscribe(int userId, BookCategory bookCategory);
    
}