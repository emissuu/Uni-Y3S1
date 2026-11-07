namespace Lab14.Interfaces;

public interface IPublisher
{
    Task Publish(INotification notification, CancellationToken cancellationToken = default);
}