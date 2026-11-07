using Lab14.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Lab14.Tests;

public class MediatorTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMediator _mediator;

    public MediatorTests()
    {
        var services = new ServiceCollection();
        services.AddMediator(typeof(NotificationsTestData).Assembly, typeof(RequestsTestData).Assembly);
        _serviceProvider = services.BuildServiceProvider();
        
        _mediator = _serviceProvider.GetRequiredService<IMediator>();
    }
    
    
    
    [Fact]
    public async Task ShouldAddNumbers()
    {
        var request = new RequestsTestData.AdditionRequest(12, 15);

        var response = await _mediator.Send(request);
        
        Assert.Equal(27, response);
    }
    
    [Fact]
    public async Task ShouldMultiplyNumbers()
    {
        var request = new RequestsTestData.MultiplicationRequest(2, 15);

        var response = await _mediator.Send(request);
        
        Assert.Equal(30, response);
    }

    [Fact]
    public async Task ShouldThrowWhenNoHandler()
    {
        var request = new RequestsTestData.RequestWithoutHandler("hi");

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _mediator.Send(request));
    }

    [Fact]
    public async Task ShouldThrowWhenCancellationTokenIsCancelled()
    {
        var request = new RequestsTestData.AdditionRequest(12, 15);
        var cts = new CancellationTokenSource();
        cts.Cancel();
        
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await _mediator.Send(request, cts.Token));
    }

    [Fact]
    public async Task ShouldNotifyAllSubscribers()
    {
        var count = NotificationsTestData.Count;
        var notification = new NotificationsTestData.BookAddedNotification("The Book");
        
        await _mediator.Publish(notification);
        
        Assert.Equal(count + 3, NotificationsTestData.Count);
    }
}