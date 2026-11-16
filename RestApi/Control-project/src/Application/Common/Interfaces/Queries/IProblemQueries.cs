using Domain.Problems;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IProblemQueries
{
    Task<IReadOnlyList<Problem>> GetAll(CancellationToken cancellationToken); 
    Task<Option<Problem>> GetById(ProblemId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Problem>> GetFiltered(
        string? title,
        string? code,
        string? answer,
        Category? category,
        Difficulty? difficulty,
        DateTime? createdAtFrom,
        DateTime? createdAtTo,
        CancellationToken cancellationToken);
}