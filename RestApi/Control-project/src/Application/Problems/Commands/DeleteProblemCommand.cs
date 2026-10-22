using Application.Common.Interfaces.Repositories;
using Application.Problems.Exceptions;
using Domain.Problems;
using LanguageExt;
using MediatR;

namespace Application.Problems.Commands;

public record DeleteProblemCommand : IRequest<Either<ProblemException, Problem>>
{
    public required Guid ProblemId { get; init; }
}

public class DeleteProblemCommandHandler(IProblemRepository problemRepository)
    : IRequestHandler<DeleteProblemCommand, Either<ProblemException, Problem>>
{
    public async Task<Either<ProblemException, Problem>> Handle(
        DeleteProblemCommand request,
        CancellationToken cancellationToken)
    {
        var problemId = new ProblemId(request.ProblemId);
        var problem = await problemRepository.GetById(problemId, cancellationToken);

        return await problem.MatchAsync(
            p => DeleteEntity(p, cancellationToken),
            () => new ProblemNotFoundException(problemId));
    }

    private async Task<Either<ProblemException, Problem>> DeleteEntity(
        Problem problem,
        CancellationToken cancellationToken)
    {
        try
        {
            await problemRepository.Delete(problem, cancellationToken);
            return problem;
        }
        catch (Exception exception)
        {
            return new ProblemUnhandledException(problem.Id, exception);
        }
    }
}
