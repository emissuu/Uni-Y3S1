namespace Domain.Problems;

public record ProblemId(Guid Value)
{
    public static ProblemId Empty() => new(Guid.Empty);
    public static ProblemId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}