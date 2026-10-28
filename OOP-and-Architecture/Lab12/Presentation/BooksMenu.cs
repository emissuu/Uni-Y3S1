using Lab12.Domain;
using Lab12.Interfaces;

namespace Lab12.Presentation;

public class BooksMenu(IBookService bookService)
{
    public void Run()
    {
        while (true)
        {
            Console.WriteLine(
                "Books manager\n" +
                "1 - List all books\n" +
                "2 - Add new book\n" +
                "3 - Change a book\n" +
                "4 - Remove a book\n" +
                "0 - Back");

            var option = InputReader.ReadInt("Choose action: ");

            switch (option)
            {
                case 1:
                    Console.WriteLine("Books: ");
                    foreach (var book in bookService.GetAll())
                        Console.WriteLine($"{book.Id}. {book.Title} by {book.Author}. {book.Category}");
                    break;
                
                case 2:
                    var title = InputReader.ReadString("Enter title: ");
                    var author = InputReader.ReadString("Enter author: ");
                    var category = InputReader.ReadEnum<BookCategory>("Enter category: ");
                    
                    bookService.Add(title, author, category);
                    Console.WriteLine("Book added successfully!");
                    break;
                
                case 3:
                    var bookIdToUpdate = InputReader.ReadInt("Enter book id: ");
                    var newTitle = InputReader.ReadString("Enter new title: ");
                    var newAuthor = InputReader.ReadString("Enter new author: ");
                    var newCategory = InputReader.ReadEnum<BookCategory>("Enter new category: ");
                    
                    var updated = bookService.Update(bookIdToUpdate, newTitle, newAuthor, newCategory);
                    if (updated) 
                        Console.WriteLine("Book updated successfully!");
                    else  
                        Console.WriteLine("Book not found.");
                    break;
                
                case 4:
                    var bookIdToRemove = InputReader.ReadInt("Enter book id: ");
                    
                    var removed = bookService.Delete(bookIdToRemove);
                    if (removed) 
                        Console.WriteLine("Book removed successfully!");
                    else  
                        Console.WriteLine("Book not found.");
                    break;
                
                case 0:
                    return;
            }
        }
    }
}