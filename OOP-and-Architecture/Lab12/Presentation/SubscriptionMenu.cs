using Lab12.Domain;
using Lab12.Interfaces;

namespace Lab12.Presentation;

public class SubscriptionMenu(IUserService userService)
{
    public void Run()
    {
        while (true)
        {
            Console.WriteLine(
                "1 - List current subscriptions\n" +
                "2 - Subscribe to a category\n" +
                "3 - Unsubscribe from a category\n" +
                "0 - Back");

            var option = InputReader.ReadInt("Choose action: ");

            switch (option)
            {
                case 1:
                    var userIdToList = InputReader.ReadInt("Enter user id: ");
                    var userToList = userService.GetById(userIdToList);

                    if (userToList == null)
                    {
                        Console.WriteLine("User not found.");
                        break;
                    }

                    if (userToList.SubscribedCategories.Any())
                    {
                        Console.WriteLine("Users current subscriptions:");
                        foreach (var subscription in userToList.SubscribedCategories)
                            Console.WriteLine(subscription);
                    }
                    else
                        Console.WriteLine("User doesn't have any active subscription.");

                    break;

                case 2:
                    var userIdToSub = InputReader.ReadInt("Enter user id: ");
                    var userToSub = userService.GetById(userIdToSub);

                    if (userToSub == null)
                    {
                        Console.WriteLine("User not found.");
                        break;
                    }

                    var categoryToSub = InputReader.ReadEnum<BookCategory>("Enter category: ");
                    var added = userService.Subscribe(userIdToSub, categoryToSub);
                    if (added)
                        Console.WriteLine($"User subscribed to {categoryToSub} successfully.");
                    else
                        Console.WriteLine($"Could not subscribe user to {categoryToSub}.");
                    return;

                case 3:
                    var userIdToUnsub = InputReader.ReadInt("Enter user id: ");
                    var userToUnsub = userService.GetById(userIdToUnsub);

                    if (userToUnsub == null)
                    {
                        Console.WriteLine("User not found.");
                        break;
                    }

                    var categoryToUnsub = InputReader.ReadEnum<BookCategory>("Enter category: ");
                    var removed = userService.Subscribe(userIdToUnsub, categoryToUnsub);
                    if (removed)
                        Console.WriteLine($"User unsubscribed from {categoryToUnsub} successfully.");
                    else
                        Console.WriteLine($"Could not unsubscribe user from {categoryToUnsub}.");
                    return;

                case 0:
                    return;
            }
        }
    }
}