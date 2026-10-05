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
    [Required, MaxLength(255)] string Question,
    [MaxLength(2047)] string? Hint,
    [Required, MaxLength(255)] string Answer,
    [MaxLength(2047)] string? Description
);

public record UpdateFlashcardDto(
    [Required, MaxLength(255)] string Question,
    [MaxLength(2047)] string? Hint,
    [Required, MaxLength(255)] string Answer,
    [MaxLength(2047)] string? Description,
    [Range(1, 100_000)] int Score
);