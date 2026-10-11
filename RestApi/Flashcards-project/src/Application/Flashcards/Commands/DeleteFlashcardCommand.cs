using Application.Common.Interfaces.Repositories;
using Application.Flashcards.Exceptions;
using Domain.Flashcards;
using LanguageExt;
using MediatR;

namespace Application.Flashcards.Commands;

public record DeleteFlashcardCommand : IRequest<Either<FlashcardException, Flashcard>>
{
    public required Guid FlashcardId { get; init; }
}

public class DeleteFlashcardCommandHandler(IFlashcardRepository flashcardRepository)
    : IRequestHandler<DeleteFlashcardCommand, Either<FlashcardException, Flashcard>>
{
    public async Task<Either<FlashcardException, Flashcard>> Handle(
        DeleteFlashcardCommand request, 
        CancellationToken cancellationToken)
    {
        var flashcardId = new FlashcardId(request.FlashcardId);
        var flashcard = await flashcardRepository.GetById(flashcardId, cancellationToken);

        return await flashcard.MatchAsync(
            f => DeleteEntity(f, cancellationToken),
            () => new FlashcardNotFoundException(flashcardId));
    }

    private async Task<Either<FlashcardException, Flashcard>> DeleteEntity(
        Flashcard flashcard,
        CancellationToken cancellationToken)
    {
        try
        {
            await flashcardRepository.Delete(flashcard, cancellationToken);
            return flashcard;
        }
        catch (Exception exception)
        {
            return new FlashcardUnhandledException(flashcard.Id, exception);
        }
    }
}