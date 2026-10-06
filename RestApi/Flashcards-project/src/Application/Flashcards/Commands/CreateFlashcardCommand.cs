using Application.Common.Interfaces.Repositories;
using Domain.Flashcards;
using MediatR;

namespace Application.Flashcards.Commands;

public record CreateFlashcardCommand : IRequest<Flashcard>
{
    public required string Question { get; init; }
    public string? Hint { get; init; }
    public required string Answer { get; init; }
    public string? Description { get; init; } 
}

public class CreateFlashcardCommandHandler(IFlashcardRepository flashcardRepository)
     : IRequestHandler<CreateFlashcardCommand, Flashcard>
{
    public async Task<Flashcard> Handle(CreateFlashcardCommand request, CancellationToken cancellationToken)
    {
        var exisitingFlashcard = await flashcardRepository.GetByQuestion(request.Question, cancellationToken);
        if (exisitingFlashcard is not null)
        {
            throw new ArgumentException($"Flashcard with question '{request.Question}' already exists");
        }
        
        var flashcard = Flashcard.New(Guid.NewGuid(), request.Question, request.Hint, request.Answer, request.Description, 72, DateTime.UtcNow);
        return await flashcardRepository.Add(flashcard, cancellationToken);
    }
}