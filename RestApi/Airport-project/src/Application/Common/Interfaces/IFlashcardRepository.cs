using Domain.Flashcards;

namespace Application.Common.Interfaces;

public interface IFlashcardRepository
{
    Task<IReadOnlyList<Flashcard>> GetAll(CancellationToken cancellationToken);
    Task<Flashcard?> GetById(Guid id, CancellationToken cancellationToken);
    Task<Flashcard?> GetByQuestion(string question, CancellationToken cancellationToken);
    Task<Flashcard> Add(Flashcard flashcard, CancellationToken cancellationToken);
    Task<Flashcard> Update(Flashcard flashcard, CancellationToken cancellationToken);
    Task Delete(Flashcard flashcard, CancellationToken cancellationToken);
}