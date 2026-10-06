using Domain.Flashcards;

namespace Application.Common.Interfaces.Queries;

public interface IFlashcardQueries
{
    Task<IReadOnlyList<Flashcard>> GetAll(CancellationToken cancellationToken);
    Task<Flashcard?> GetById(Guid id, CancellationToken cancellationToken);
}