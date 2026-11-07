using Lab14.Interfaces;

namespace Lab14.Tests;

public static class RequestsTestData
{
    public record AdditionRequest(int A, int B) : IRequest<int>;

    public class AdditionRequestHandler : IRequestHandler<AdditionRequest, int>
    {
        public Task<int> Handle(AdditionRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(request.A + request.B);
        }
    }
    
    public record MultiplicationRequest(int A, int B) : IRequest<int>;
    
    public class MultiplicationRequestHandler : IRequestHandler<MultiplicationRequest, int>
    {
        public Task<int> Handle(MultiplicationRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(request.A * request.B);
        }
    }
    
    public record RequestWithoutHandler(string message) : IRequest<string>;
}