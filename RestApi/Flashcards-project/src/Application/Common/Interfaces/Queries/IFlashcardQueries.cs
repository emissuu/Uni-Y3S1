using Domain.Flashcards;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IFlashcardQueries
{
    Task<IReadOnlyList<Flashcard>> GetAll(CancellationToken cancellationToken);
    Task<Option<Flashcard>> GetById(FlashcardId id, CancellationToken cancellationToken);
}