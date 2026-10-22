namespace Domain.Problems;

public class Problem
{
    public ProblemId Id { get; }
    public string Title { get; private set; }
    public string Code { get; private set; }
    public string Answer { get; private set; }
    public Category Category { get; private set;  }
    public Difficulty Difficulty { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private Problem(
        ProblemId id,
        string title,
        string code,
        string answer,
        Category category,
        Difficulty difficulty,
        DateTime createdAt) => 
    (Id, Title, Code, Answer, Category, Difficulty, CreatedAt) = 
    (id, title, code, answer, category, difficulty, createdAt);
    
    public static Problem New(
        ProblemId id,
        string title,
        string code,
        Category category,
        Difficulty difficulty,
        string answer) =>
    new Problem(id, title, code, answer, category, difficulty, DateTime.UtcNow);
    
    public void UpdateDetails(
        string title,
        string code,
        Category category,
        Difficulty difficulty,
        string answer) =>
    (Title, Category, Difficulty, Code, Answer, UpdatedAt) =
    (title, category, difficulty, code, answer, DateTime.UtcNow);
}