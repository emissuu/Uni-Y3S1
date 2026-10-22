using Application.Common.Interfaces.Repositories;
using Application.Problems.Exceptions;
using Domain.Problems;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Problems.Commands;

public record UpdateProblemCommand : IRequest<Either<ProblemException, Problem>>
{
    public required Guid ProblemId { get; init; }
    public required string Title { get; init; }
    public required string Code { get; init; }
    public required Category Category { get; init; }
    public required Difficulty Difficulty { get; init; }
    public required string Answer { get; init; }
}

public class UpdateProblemCommandHandler(IProblemRepository problemRepository)
    : IRequestHandler<UpdateProblemCommand, Either<ProblemException, Problem>>
{
    public async Task<Either<ProblemException, Problem>> Handle(
        UpdateProblemCommand request,
        CancellationToken cancellationToken)
    {
        var problemId = new ProblemId(request.ProblemId);
        var problem = await problemRepository.GetById(problemId, cancellationToken);
        return await problem.MatchAsync(
            p => CheckDuplicates(p.Id, request.Title, cancellationToken)
                .BindAsync(_ => UpdateEntity(p, request, cancellationToken)),
            () => new ProblemNotFoundException(problemId));
    }

    private async Task<Either<ProblemException, Unit>> CheckDuplicates(
        ProblemId problemId,
        string problemTitle,
        CancellationToken cancellationToken)
    {
        var problem = await problemRepository.GetByTitle(problemTitle, cancellationToken);
        return problem.Match<Either<ProblemException, Unit>>(
            p => p.Id == problemId ? Unit.Default : new ProblemAlreadyExistsException(p.Id),
            () => Unit.Default);
    }

    private async Task<Either<ProblemException, Problem>> UpdateEntity(
        Problem problem,
        UpdateProblemCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            problem.UpdateDetails(
                request.Title,
                request.Code,
                request.Category,
                request.Difficulty,
                request.Answer);
            
            return await problemRepository.Update(problem, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ProblemUnhandledException(problem.Id, exception);
        }
    }
}