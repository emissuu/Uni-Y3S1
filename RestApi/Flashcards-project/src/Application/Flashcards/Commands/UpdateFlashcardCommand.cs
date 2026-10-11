using Application.Common.Interfaces.Repositories;
using Application.Flashcards.Exceptions;
using Domain.Flashcards;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Flashcards.Commands;

public record UpdateFlashcardCommand : IRequest<Either<FlashcardException, Flashcard>>
{
    public required Guid FlashcardId { get; init; }
    public required string Question { get; init; }
    public string? Hint { get; init; }
    public required string Answer { get; init; }
    public string? Description { get; init; }
    public int? Score { get; init; }
    public DateTime? DueDate { get; init; }
}

public class UpdateFlashcardCommandHandler(IFlashcardRepository flashcardRepository)
    : IRequestHandler<UpdateFlashcardCommand, Either<FlashcardException, Flashcard>>
{
    public async Task<Either<FlashcardException, Flashcard>> Handle(
        UpdateFlashcardCommand request,
        CancellationToken cancellationToken)
    {
        var flashcardId = new FlashcardId(request.FlashcardId);
        var flashcard = await flashcardRepository.GetById(flashcardId, cancellationToken);

        return await flashcard.MatchAsync(
            f => CheckDuplicates(f.Id, request.Question, cancellationToken)
                .BindAsync(_ => UpdateEntity(f, request, cancellationToken)),
            () => new FlashcardNotFoundException(flashcardId));
    }

    private async Task<Either<FlashcardException, Unit>> CheckDuplicates(
        FlashcardId currentFlashcardId,
        string question,
        CancellationToken cancellationToken)
    {
        var flashcard = await flashcardRepository.GetByQuestion(question, cancellationToken);
        return flashcard.Match<Either<FlashcardException, Unit>>(
            f => f.Id == currentFlashcardId ? Unit.Default : new FlashcardAlreadyExistsException(f.Id),
            () => Unit.Default);
    }

    private async Task<Either<FlashcardException, Flashcard>> UpdateEntity(
        Flashcard flashcard,
        UpdateFlashcardCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            flashcard.UpdateDetails(
                request.Question,
                request.Hint,
                request.Answer,
                request.Description,
                request.Score ?? flashcard.Score,
                request.DueDate ?? flashcard.DueDate);

            return await flashcardRepository.Update(flashcard, cancellationToken);
        }
        catch (Exception exception)
        {
            return new FlashcardUnhandledException(flashcard.Id, exception);
        }
    }
}