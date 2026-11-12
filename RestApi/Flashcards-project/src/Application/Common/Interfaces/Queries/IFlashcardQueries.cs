using Domain.Flashcards;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IFlashcardQueries
{
    Task<IReadOnlyList<Flashcard>> GetAll(CancellationToken cancellationToken);
    Task<Option<Flashcard>> GetById(FlashcardId id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Flashcard>> GetFiltered(
        string? question,
        string? hint,
        string? answer,
        string? description,
        int? score,
        DateTime? dueDateFrom,
        DateTime? dueDateTo,
        DateTime? createdAtFrom,
        DateTime? createdAtTo,
        CancellationToken cancellationToken);
}