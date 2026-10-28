using Lab12.Presentation;
using Lab12.Interfaces;
using Lab12.Observers;
using Lab12.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lab12;

class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IBookService, BookService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICategoryObserver, CategoryObserver>();

        services.AddTransient<MainMenu>();
        services.AddTransient<UsersMenu>();
        services.AddTransient<BooksMenu>();
        services.AddTransient<SubscriptionMenu>();
        
        var serviceProvider = services.BuildServiceProvider();
        
        var mainMenu = serviceProvider.GetRequiredService<MainMenu>();
        mainMenu.Run();
    }
}