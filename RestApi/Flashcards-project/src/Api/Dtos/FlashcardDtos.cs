using System.ComponentModel.DataAnnotations;
using Domain.Flashcards;

namespace Api.Dtos;

public record FlashcardDto(
    Guid Id,
    string Question,
    string? Hint,
    string Answer,
    string? Description,
    int Score, 
    DateTime DueDate,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt)
{
    public static FlashcardDto FromDomainModel(Flashcard flashcard)
        => new(
            flashcard.Id, 
            flashcard.Question,
            flashcard.Hint,
            flashcard.Answer,
            flashcard.Description,
            flashcard.Score,
            flashcard.DueDate, 
            flashcard.CreatedAt,
            flashcard.UpdatedAt,
            flashcard.DeletedAt);
}

public record CreateFlashcardDto(
    string Question,
    string? Hint,
    string Answer,
    string? Description
);

public record UpdateFlashcardDto(
    string Question,
    string? Hint,
    string Answer,
    string? Description,
    int Score,
    DateTime DueDate
);