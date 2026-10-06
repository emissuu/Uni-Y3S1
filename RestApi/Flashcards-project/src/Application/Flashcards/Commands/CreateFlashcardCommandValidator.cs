using FluentValidation;

namespace Application.Flashcards.Commands;

public class CreateFlashcardCommandValidator : AbstractValidator<CreateFlashcardCommand>
{
    public CreateFlashcardCommandValidator()
    {
        RuleFor(x => x.Question).NotEmpty().MinimumLength(3).MaximumLength(255);
        RuleFor(x => x.Hint).MaximumLength(2047);
        RuleFor(x => x.Answer).NotEmpty().MinimumLength(3).MaximumLength(255);
        RuleFor(x => x.Description).MaximumLength(2047);
    }
}