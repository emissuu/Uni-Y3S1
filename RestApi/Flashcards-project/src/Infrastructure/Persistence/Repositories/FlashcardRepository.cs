using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Flashcards;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class FlashcardRepository(ApplicationDbContext context) : IFlashcardRepository, IFlashcardQueries
{
    public async Task<IReadOnlyList<Flashcard>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Flashcards
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Flashcard>> GetById(FlashcardId id, CancellationToken cancellationToken)
    {
        return await context.Flashcards.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Option<Flashcard>> GetByQuestion(string question, CancellationToken cancellationToken)
    {
        return await context.Flashcards.FirstOrDefaultAsync(x => x.Question == question, cancellationToken);
    }

    public async Task<Flashcard> Add(Flashcard flashcard, CancellationToken cancellationToken)
    {
        await context.Flashcards.AddAsync(flashcard, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return flashcard;
    }

    public async Task<Flashcard> Update(Flashcard flashcard, CancellationToken cancellationToken)
    {
        context.Entry(flashcard).CurrentValues.SetValues(flashcard);
        await context.SaveChangesAsync(cancellationToken);
        return flashcard;
    }

    public async Task Delete(Flashcard flashcard, CancellationToken cancellationToken)
    {
        context.Flashcards.Remove(flashcard);
        await context.SaveChangesAsync(cancellationToken);
    }
}