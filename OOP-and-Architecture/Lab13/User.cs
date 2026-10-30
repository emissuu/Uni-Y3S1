namespace Lab13;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public User(int id, string login, string email, DateTime createdAt) => 
        (Id, Login, Email, CreatedAt) = (id, login, email, createdAt);
}