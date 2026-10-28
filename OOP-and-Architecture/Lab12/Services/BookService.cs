using Lab12.Domain;
using Lab12.Interfaces;

namespace Lab12.Services;

public class BookService(ICategoryObserver categoryObserver) : IBookService
{
    private readonly List<Book> _books = [];
    
    public IReadOnlyList<Book> GetAll()
    {
        return _books;
    }

    public Book? GetById(int id)
    {
        return _books.FirstOrDefault(b => b.Id == id);
    }

    public void Add(string title, string author, BookCategory category)
    {
        int newId = _books.Count + 1;
        var book = new Book(newId, title, author, category);
        
        _books.Add(book);
        categoryObserver.OnBookAdded(book);
    }

    public bool Update(int bookId, string title, string author, BookCategory category)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId);
        if (book == null)
            return false;
        
        book.Update(title, author, category);
        categoryObserver.OnBookUpdated(book);
        return true;
    }

    public bool Delete(int bookId)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId);
        if (book == null)
            return false;
        
        _books.Remove(book);
        categoryObserver.OnBookRemoved(book);
        return true;
    }
}