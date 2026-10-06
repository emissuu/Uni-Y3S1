using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Flashcards.Commands;

public record DeleteFlashcardCommand : IRequest<bool>
{
    public required Guid FlashcardId { get; init; }
}

public class DeleteFlashcardCommandHandler(IFlashcardRepository flashcardRepository)
    : IRequestHandler<DeleteFlashcardCommand, bool>
{
    public async Task<bool> Handle(DeleteFlashcardCommand request, CancellationToken cancellationToken)
    {
        var flashcard = await flashcardRepository.GetById(request.FlashcardId, cancellationToken);
        if (flashcard is null)
        {
            return false;
        }
        await flashcardRepository.Delete(flashcard, cancellationToken);
        return true;
    }
}