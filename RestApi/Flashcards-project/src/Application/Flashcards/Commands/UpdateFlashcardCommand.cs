using Application.Common.Interfaces.Repositories;
using Domain.Flashcards;
using MediatR;

namespace Application.Flashcards.Commands;

public record UpdateFlashcardCommand : IRequest<Flashcard?>
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
    : IRequestHandler<UpdateFlashcardCommand, Flashcard?>
{
    public async Task<Flashcard?> Handle(UpdateFlashcardCommand request, CancellationToken cancellationToken)
    {
        var existingFlashcard = await flashcardRepository.GetById(request.FlashcardId, cancellationToken);
        if (existingFlashcard is null)
        {
            return null;
        }
        
        var flashcardSameQuestion = await flashcardRepository.GetByQuestion(request.Question, cancellationToken);
        if (flashcardSameQuestion is not null && flashcardSameQuestion.Id != existingFlashcard.Id)
        {
            throw new ArgumentException($"Flashcard with question '{request.Question}' already exists");
        }
        
        existingFlashcard.UpdateDetails(
            request.Question, 
            request.Hint, 
            request.Answer,
            request.Description,
            request.Score ?? existingFlashcard.Score,
            request.DueDate ?? existingFlashcard.DueDate);
        
        return await flashcardRepository.Update(existingFlashcard, cancellationToken);
    }
}