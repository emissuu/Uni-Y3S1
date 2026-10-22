using Domain.Problems;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories;

public interface IProblemRepository
{
    Task<Option<Problem>> GetById(ProblemId id, CancellationToken cancellationToken);
    Task<Option<Problem>> GetByTitle(string title, CancellationToken cancellationToken);
    Task<Problem> Add(Problem problem, CancellationToken cancellationToken);
    Task<Problem> Update(Problem problem, CancellationToken cancellationToken);
    Task Delete(Problem problem, CancellationToken cancellationToken);
}