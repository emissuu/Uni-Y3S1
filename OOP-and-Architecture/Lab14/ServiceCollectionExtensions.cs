using System.Reflection;
using Lab14.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Lab14;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);

        if (assemblies.Length == 0)
            throw new ArgumentException("At least one assembly must be passed");

        services.AddTransient<Mediator>();
        
        services.AddTransient<IMediator>(provider => provider.GetRequiredService<Mediator>());
        services.AddTransient<ISender>(provider => provider.GetRequiredService<Mediator>());
        services.AddTransient<IPublisher>(provider => provider.GetRequiredService<Mediator>());

        var handlerInterfaces = new[]
        {
            typeof(IRequestHandler<,>),
            typeof(INotificationHandler<>)
        };

        foreach (var implementationType in assemblies
                     .Distinct()
                     .SelectMany(assembly => assembly.GetTypes())
                     .Where(x => x is { IsClass: true, IsAbstract: false }))
        {
            var implementedHandlerInterfaces = implementationType
                .GetInterfaces()
                .Where(x => x.IsGenericType && handlerInterfaces.Contains(x.GetGenericTypeDefinition()))
                .ToArray();

            foreach (var handlerInterface in implementedHandlerInterfaces)
            {
                services.AddTransient(handlerInterface, implementationType);    
            }
        }
        
        return services;
    }
}