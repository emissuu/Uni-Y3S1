using Domain.Problems;
using FluentValidation;

namespace Application.Problems.Commands;

public class CreateProblemCommandValidator : AbstractValidator<CreateProblemCommand>
{
    public CreateProblemCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(255);
        RuleFor(x => x.Code).NotEmpty().MinimumLength(1).MaximumLength(2047);
        RuleFor(x => x.Category).IsInEnum().NotNull();
        RuleFor(x => x.Difficulty).IsInEnum().NotNull();
        RuleFor(x => x.Answer).NotEmpty().MinimumLength(1).MaximumLength(255);

    }
}