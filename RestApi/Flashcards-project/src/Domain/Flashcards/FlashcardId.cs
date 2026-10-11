namespace Domain.Flashcards;

public record FlashcardId(Guid Value)
{
    public static FlashcardId Empty() => new(Guid.Empty);
    public static FlashcardId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}