namespace Lab12.Domain;

public class Book
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public BookCategory Category { get; private set; }
    
    public Book(int id, string title, string author, BookCategory category) =>
        (Id, Title, Author, Category) = (id, title, author, category);
    
    public void Update(string title, string author, BookCategory category) =>
        (Title, Author, Category) = (title, author, category);
}