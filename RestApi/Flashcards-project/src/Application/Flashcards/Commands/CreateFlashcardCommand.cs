using Application.Common.Interfaces.Repositories;
using Application.Flashcards.Exceptions;
using Domain.Flashcards;
using LanguageExt;
using MediatR;

namespace Application.Flashcards.Commands;

public record CreateFlashcardCommand : IRequest<Either<FlashcardException, Flashcard>>
{
    public required string Question { get; init; }
    public string? Hint { get; init; }
    public required string Answer { get; init; }
    public string? Description { get; init; } 
}

public class CreateFlashcardCommandHandler(IFlashcardRepository flashcardRepository)
     : IRequestHandler<CreateFlashcardCommand, Either<FlashcardException, Flashcard>>
{
    public async Task<Either<FlashcardException, Flashcard>> Handle(
        CreateFlashcardCommand request, 
        CancellationToken cancellationToken)
    {
        var flashcard = await flashcardRepository.GetByQuestion(request.Question, cancellationToken);
        return await flashcard.MatchAsync(
            f => new FlashcardAlreadyExistsException(f.Id),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<FlashcardException, Flashcard>> CreateEntity(
        CreateFlashcardCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var flashcard = Flashcard.New(
                FlashcardId.New(),
                request.Question,
                request.Hint,
                request.Answer,
                request.Description,
                72,
                DateTime.UtcNow);
            return await flashcardRepository.Add(flashcard, cancellationToken);
        }
        catch (Exception exception)
        {
            return new FlashcardUnhandledException(FlashcardId.Empty(), exception);
        }
    }
}