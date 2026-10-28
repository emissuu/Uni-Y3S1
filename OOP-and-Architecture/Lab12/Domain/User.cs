namespace Lab12.Domain;

public class User
{
    public int Id { get; private set; }
    public string Login { get; private set; }
    public string Email { get; private set; }

    private List<BookCategory> _subscribedCategories = [];
    
    public User(int id, string login, string email) =>
        (Id, Login, Email) = (id, login, email);
    
    public IReadOnlyList<BookCategory> SubscribedCategories => _subscribedCategories;

    public void Update(string login, string email) =>
        (Login, Email) = (login, email);
    
    public void Subscribe(BookCategory category)
    {
        if (!_subscribedCategories.Contains(category))
            _subscribedCategories.Add(category);
    }

    public void Unsubscribe(BookCategory category)
    {
        if (_subscribedCategories.Contains(category))
            _subscribedCategories.Remove(category);
    }
}