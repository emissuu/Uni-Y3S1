using Lab12.Domain;

namespace Lab12.Interfaces;

public interface ICategoryObserver
{
    void OnBookAdded(Book book);
    void OnBookUpdated(Book book);
    void OnBookRemoved(Book book);
}