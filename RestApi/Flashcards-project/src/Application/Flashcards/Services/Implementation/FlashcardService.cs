using Application.Common.Interfaces;
using Application.Flashcards.Services.Abstract;
using Domain.Flashcards;

namespace Application.Flashcards.Services.Implementation;

public class FlashcardService(IFlashcardRepository flashcardRepository) : IFlashcardService
{
    public async Task<IReadOnlyList<Flashcard>> GetFlashcards(CancellationToken cancellationToken)
    {
        return await flashcardRepository.GetAll(cancellationToken);
    }

    public async Task<Flashcard?> GetFlashcard(Guid id, CancellationToken cancellationToken)
    {
        return await flashcardRepository.GetById(id, cancellationToken);
    }

    public async Task<Flashcard> Add(string question, string? hint, string answer, string? description,
        CancellationToken cancellationToken)
    {
        var existingFlashcard = await flashcardRepository.GetByQuestion(question, cancellationToken);
        if (existingFlashcard is not null)
        {
            throw new ArgumentException($"Flashcard with that question already exists");
        }

        if (String.IsNullOrWhiteSpace(question) || question.Length < 3)
        {
            throw new ArgumentException($"Flashcard question must have at least 3 characters");
        }
        if (String.IsNullOrWhiteSpace(answer) || answer.Length < 3)
        {
            throw new ArgumentException($"Flashcard answer must have at least 3 characters");
        }
        
        var flashcard = Flashcard.New(Guid.NewGuid(), question, hint, answer, description, 72, DateTime.UtcNow);
        return await flashcardRepository.Add(flashcard, cancellationToken);
    }

    public async Task<Flashcard?> Update(Guid id, string question, string? hint, string answer, string? description, 
        int score, CancellationToken cancellationToken)
    {
        var flashcard = await flashcardRepository.GetById(id, cancellationToken);
        if (flashcard is null)
        {
            return null;
        }
        
        var flashcardSameQuestion = await flashcardRepository.GetByQuestion(question, cancellationToken);
        if (flashcardSameQuestion is not null && flashcardSameQuestion.Id != flashcard.Id)
        {
            throw new ArgumentException($"Flashcard with that question already exists");
        }
        if (String.IsNullOrWhiteSpace(question) || question.Length < 3)
        {
            throw new ArgumentException($"Flashcard question must have at least 3 characters");
        }
        if (String.IsNullOrWhiteSpace(answer) || answer.Length < 3)
        {
            throw new ArgumentException($"Flashcard answer must have at least 3 characters");
        }
        if (score < 1 || score > 1e6)
        {
            throw new ArgumentException($"Flashcard score must be between 1 and 1,000,000");
        }
        flashcard.UpdateDetails(question, hint, answer, description, score, flashcard.DueDate.AddHours(score));
        return await flashcardRepository.Update(flashcard, cancellationToken);
    }

    public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
    {
        var flashcard = await flashcardRepository.GetById(id, cancellationToken);
        if (flashcard is null)
        {
            return false;
        }
        await flashcardRepository.Delete(flashcard, cancellationToken);
        return true;
    }
}