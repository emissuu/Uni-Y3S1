using Lab12.Domain;

namespace Lab12.Interfaces;

public interface IBookService
{
    IReadOnlyList<Book> GetAll();
    Book? GetById(int id);
    void Add(string title, string author, BookCategory category);
    bool Update(int bookId, string title, string author, BookCategory category);
    bool Delete(int bookId);
}