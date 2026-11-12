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

    public async Task<IReadOnlyList<Flashcard>> GetFiltered(
        string? question,
        string? hint,
        string? answer,
        string? description,
        int? score,
        DateTime? dueDateFrom,
        DateTime? dueDateTo,
        DateTime? createdAtFrom,
        DateTime? createdAtTo,
        CancellationToken cancellationToken)
    {
        var query = context.Flashcards.AsQueryable();

        if (!String.IsNullOrEmpty(question))
        {
            query = query.Where(x => x.Question.Contains(question));
        }

        if (!String.IsNullOrEmpty(hint))
        {
            query = query.Where(x => x.Hint != null && x.Hint.Contains(hint));
        }

        if (!String.IsNullOrEmpty(answer))
        {
            query = query.Where(x => x.Answer.Contains(answer));
        }

        if (!String.IsNullOrEmpty(description))
        {
            query = query.Where(x => x.Description != null && x.Description.Contains(description));
        }

        if (score.HasValue)
        {
            query = query.Where(x => x.Score == score);
        }

        if (dueDateFrom.HasValue)
        {
            query = query.Where(x => x.DueDate >= dueDateFrom);
        }
        
        if (dueDateTo.HasValue)
        {
            query = query.Where(x => x.DueDate <= dueDateTo);
        }

        if (createdAtFrom.HasValue)
        {
            query = query.Where(x => x.CreatedAt >= createdAtFrom);
        }

        if (createdAtTo.HasValue)
        {
            query = query.Where(x => x.CreatedAt <= createdAtTo);
        }
        
        return await query.ToListAsync(cancellationToken);
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