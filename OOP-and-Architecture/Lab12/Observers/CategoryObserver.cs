using Lab12.Domain;
using Lab12.Interfaces;

namespace Lab12.Observers;

public class CategoryObserver(IEmailService emailService, IUserService userService)
    : ICategoryObserver
{
    public void OnBookAdded(Book book)
    {
        foreach (var user in GetSubscribedUsers(book.Category))
        {
            emailService.SendEmail(
                user.Email,
                $"New book in {book.Category}",
                $"Dear {user.Login}! {book.Title} by {book.Author} has just been added to {book.Category} category.");
        }
    }

    public void OnBookUpdated(Book book)
    {
        foreach (var user in GetSubscribedUsers(book.Category))
        {
            emailService.SendEmail(
                user.Email,
                $"Book changed in {book.Category}",
                $"Dear {user.Login}! Information about {book.Title} by {book.Author} has just been changed in {book.Category} category.");
        }
    }

    public void OnBookRemoved(Book book)
    {
        foreach (var user in GetSubscribedUsers(book.Category))
        {
            emailService.SendEmail(
                user.Email,
                $"Book removed from {book.Category}",
                $"Dear {user.Login}! {book.Title} by {book.Author} has just been removed from {book.Category} category.");
        }
    }

    private IReadOnlyList<User> GetSubscribedUsers(BookCategory bookCategory)
    {
        return userService.GetAll()
            .Where(x => x.SubscribedCategories.Contains(bookCategory))
            .ToList();
    }
}