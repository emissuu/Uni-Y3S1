using Domain.Flashcards;

namespace Application.Common.Interfaces.Repositories;

public interface IFlashcardRepository
{
    Task<Flashcard?> GetById(Guid id, CancellationToken cancellationToken);
    Task<Flashcard?> GetByQuestion(string question, CancellationToken cancellationToken);
    Task<Flashcard> Add(Flashcard flashcard, CancellationToken cancellationToken);
    Task<Flashcard> Update(Flashcard flashcard, CancellationToken cancellationToken);
    Task Delete(Flashcard flashcard, CancellationToken cancellationToken);
}