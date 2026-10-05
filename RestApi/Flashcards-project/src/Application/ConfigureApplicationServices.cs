using Application.Flashcards.Services.Abstract;
using Application.Flashcards.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IFlashcardService, FlashcardService>();
    }
}