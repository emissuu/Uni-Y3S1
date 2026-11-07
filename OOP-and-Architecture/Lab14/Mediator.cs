using Lab14.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Lab14;

public sealed class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken.ThrowIfCancellationRequested();
        
        Type requestType = request.GetType();
        Type handlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(requestType, typeof(TResponse));

        object handler;
        try
        {
            handler = serviceProvider.GetRequiredService(handlerType);
        }
        catch (InvalidOperationException exception)
        {
            throw new InvalidOperationException($"Handler for {requestType.Name} was not found", exception);
        }

        var handlerMethod = handlerType.GetMethod(nameof(IRequestHandler<,>.Handle))
            ?? throw new InvalidOperationException("Couldn't find the handle method");

        var task = handlerMethod.Invoke(
            handler,
            [request, cancellationToken]) as Task<TResponse> 
                ?? throw new InvalidOperationException("Handler returned invalid response.");
        
        return await task.ConfigureAwait(false);
    }

    public async Task Publish(INotification notification, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(notification);
        cancellationToken.ThrowIfCancellationRequested();

        Type notificationType = notification.GetType();
        Type handlerType = typeof(INotificationHandler<>)
            .MakeGenericType(notificationType);
        
        var handlers = serviceProvider.GetServices(handlerType);
        
        var handleMethod = handlerType.GetMethod(nameof(INotificationHandler<>.Handle))
            ?? throw new InvalidOperationException("Couldn't find the handle method");

        foreach (var handler in handlers)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var task = handleMethod.Invoke(handler, [notification, cancellationToken]) as Task
                ?? throw new InvalidOperationException("Handler returned invalid response");

            await task.ConfigureAwait(false);
        }
        
    }
}