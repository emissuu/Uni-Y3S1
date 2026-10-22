using Domain.Problems;

namespace Api.Dtos;

public record ProblemDto(
    Guid Id,
    string Title,
    string Code,
    Category Category,
    Difficulty Difficulty,
    string Answer,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt)
{
    public static ProblemDto FromDomainModel(Problem problem)
        => new(
            problem.Id.Value,
            problem.Title,
            problem.Code,
            problem.Category,
            problem.Difficulty,
            problem.Answer,
            problem.CreatedAt,
            problem.UpdatedAt,
            problem.DeletedAt);
}

public record CreateProblemDto(
    string Title,
    string Code,
    Category Category,
    Difficulty Difficulty,
    string Answer);
    
public record UpdateProblemDto(
    string Title,
    string Code,
    Category Category,
    Difficulty Difficulty,
    string Answer);