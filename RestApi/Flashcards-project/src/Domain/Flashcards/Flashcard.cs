namespace Domain.Flashcards;

public class Flashcard
{
    public Guid Id { get; }
    public string Question { get; private set; }
    public string? Hint { get; private set; }
    public string Answer { get; private set; }
    public string? Description { get; private set; }
    public int Score { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private Flashcard(
        Guid id,
        string question,
        string? hint,
        string answer,
        string? description,
        int score,
        DateTime dueDate,
        DateTime createdAt) =>
    (Id,  Question, Hint, Answer, Description, Score, DueDate, CreatedAt) =
    (id, question, hint, answer, description, score, dueDate, createdAt);

    public static Flashcard New(
        Guid id,
        string question,
        string? hint,
        string answer,
        string? description,
        int score,
        DateTime dueDate) =>
        new Flashcard(id, question, hint, answer, description, score, dueDate, DateTime.UtcNow);
    
    public void UpdateDetails(
        string question,
        string? hint,
        string answer,
        string? description,
        int score,
        DateTime dueDate) =>
        (Question, Hint, Answer, Description, Score, DueDate, UpdatedAt) =
        (question, hint, answer, description, score, dueDate, DateTime.UtcNow);
}