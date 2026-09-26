using Domain.Flashcards;

namespace Application.Flashcards.Services.Abstract;

public interface IFlashcardService
{
    Task<IReadOnlyList<Flashcard>> GetFlashcards(CancellationToken cancellationToken);
    Task<Flashcard?> GetFlashcard(Guid id, CancellationToken cancellationToken);
    Task<Flashcard> Add(
        string question,
        string? hint,
        string answer,
        string? description,
        CancellationToken cancellationToken);
    Task<Flashcard?> Update(
        Guid id, 
        string question,
        string? hint,
        string answer,
        string? description,
        int score,
        CancellationToken cancellationToken);
    Task<bool> Delete(Guid id, CancellationToken cancellationToken);
}