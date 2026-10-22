using Application.Common.Interfaces.Repositories;
using Application.Problems.Exceptions;
using Domain.Problems;
using LanguageExt;
using MediatR;

namespace Application.Problems.Commands;

public record CreateProblemCommand : IRequest<Either<ProblemException, Problem>>
{
    public required string Title { get; init; }
    public required string Code { get; init; }
    public required Category Category { get; init; }
    public required Difficulty Difficulty { get; init; }
    public required string Answer { get; init; }
}

public class CreateProblemCommandHandler(IProblemRepository problemRepository)
    : IRequestHandler<CreateProblemCommand, Either<ProblemException, Problem>>
{
    public async Task<Either<ProblemException, Problem>> Handle(
        CreateProblemCommand request,
        CancellationToken cancellationToken)
    {
        var problem = await problemRepository.GetByTitle(request.Title, cancellationToken);
        return await problem.MatchAsync(
            p => new ProblemAlreadyExistsException(p.Id),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<ProblemException, Problem>> CreateEntity(
        CreateProblemCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var problem = Problem.New(
                ProblemId.New(),
                request.Title,
                request.Code,
                request.Category,
                request.Difficulty,
                request.Answer);
            return await problemRepository.Add(problem, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ProblemUnhandledException(ProblemId.Empty(), exception);
        }
    }
}