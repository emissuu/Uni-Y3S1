using Lab14.Interfaces;

namespace Lab14.Tests;

public static class NotificationsTestData
{
    public static int Count = 0;
    public record BookAddedNotification(string Title) : INotification;

    public class FirstBookAddedNotificationHandler : INotificationHandler<BookAddedNotification>
    {
        public Task Handle(BookAddedNotification notification, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Count++;
            
            return Task.CompletedTask;
        }
    }
    
    public class SecondBookAddedNotificationHandler : INotificationHandler<BookAddedNotification>
    {
        public Task Handle(BookAddedNotification notification, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Count += 2;
            
            return Task.CompletedTask;
        }
    }
}