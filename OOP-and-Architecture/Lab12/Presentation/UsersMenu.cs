using Lab12.Interfaces;

namespace Lab12.Presentation;

public class UsersMenu(SubscriptionMenu subscriptionMenu, IUserService userService)
{
    public void Run()
    {
        while (true)
        {
            Console.WriteLine(
                "Users management\n" +
                "1 - List all users\n" +
                "2 - Add new user\n" +
                "3 - Change user\n" +
                "4 - Delete user\n" +
                "5 - Manage users subscription\n" +
                "0 - Back");

            var option = InputReader.ReadInt("Choose action: ");

            switch (option)
            {
                case 1:
                    Console.WriteLine("Users: ");
                    foreach (var user in userService.GetAll())
                        Console.WriteLine($"{user.Id}. {user.Login} - {user.Email}");
                    break;

                case 2:
                    var login = InputReader.ReadString("Enter login: ");
                    var email = InputReader.ReadString("Enter email: ");

                    userService.Add(login, email);
                    Console.WriteLine("User added successfully!");
                    break;

                case 3:
                    var userIdToUpdate = InputReader.ReadInt("Enter user id: ");
                    var newLogin = InputReader.ReadString("Enter new login: ");
                    var newEmail = InputReader.ReadString("Enter new email: ");

                    var updated = userService.Update(userIdToUpdate, newLogin, newEmail);
                    if (updated)
                        Console.WriteLine("User updated successfully!");
                    else
                        Console.WriteLine("User not found.");
                    break;

                case 4:
                    var userIdToRemove = InputReader.ReadInt("Enter user id: ");

                    var removed = userService.Delete(userIdToRemove);
                    if (removed)
                        Console.WriteLine("User removed successfully!");
                    else
                        Console.WriteLine("User not found.");
                    break;

                case 5:
                    subscriptionMenu.Run();
                    break;

                case 0:
                    return;
            }
        }
    }
}