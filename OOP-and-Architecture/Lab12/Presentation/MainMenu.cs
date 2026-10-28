namespace Lab12.Presentation;

public class MainMenu(BooksMenu booksMenu, UsersMenu usersMenu)
{
    public void Run()
    {
        while (true)
        {
            Console.WriteLine(
                "Welcome to the library!\n" +
                "1 - Manage books\n" +
                "2 - Manage users\n" +
                "0 - Exit");

            var option = InputReader.ReadInt("Choose action: ");

            switch (option)
            {
                case 1:
                    booksMenu.Run();
                    break;
                
                case 2:
                    usersMenu.Run();
                    break;
                
                case 0:
                    Console.WriteLine("Goodbye!");
                    return;
            }
        }
    }
}