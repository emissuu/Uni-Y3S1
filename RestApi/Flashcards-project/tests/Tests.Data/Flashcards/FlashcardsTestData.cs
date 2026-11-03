using Domain.Flashcards;

namespace Tests.Data.Flashcards;

public static class FlashcardsTestData
{
    public static Flashcard CreateValidTestFlashcard() => Flashcard.New(
        id: FlashcardId.New(),
        question: "First test flashcard",
        hint: null,
        answer: "First test answer",
        description: null,
        score: 72,
        dueDate: DateTime.UtcNow);
    
    public static Flashcard CreateTakenTestFlashcard() => Flashcard.New(
        id: FlashcardId.New(),
        question: "Second test flashcard",
        hint: null,
        answer: "Second test answer",
        description: "Second test description",
        score: 72,
        dueDate: DateTime.UtcNow);

    public static Flashcard CreateInvalidTestFlashcard() => Flashcard.New(
        id: FlashcardId.New(),
        question: "w",
        hint: null,
        answer: "te",
        description: null,
        score: 72,
        dueDate: DateTime.UtcNow);
    
    public static Flashcard UpdateValidTestFlashcard() => Flashcard.New(
        id: FlashcardId.New(),
        question: "Updated test flashcard",
        hint: null,
        answer: "Updated test answer",
        description: "Updated test description",
        score: 72,
        dueDate: DateTime.UtcNow.AddDays(3));
    
    public static Flashcard UpdateInvalidTestFlashcard() => Flashcard.New(
        id: FlashcardId.New(),
        question: "uu",
        hint: null,
        answer: "tea",
        description: null,
        score: -2,
        dueDate: DateTime.UtcNow);
    
    public static Flashcard UpdateTakenTestFlashcard() => Flashcard.New(
        id: FlashcardId.New(),
        question: "Taken test flashcard",
        hint: null,
        answer: "Taken test answer",
        description: "Taken test description",
        score: 72,
        dueDate: DateTime.UtcNow);

    public static FlashcardId NonExistentTestFlashcardId() => FlashcardId.New();
}