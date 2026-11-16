using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Problems;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ProblemRepository(ApplicationDbContext context) : IProblemRepository, IProblemQueries
{
    public async Task<IReadOnlyList<Problem>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Problems
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Problem>> GetById(ProblemId id, CancellationToken cancellationToken)
    {
        return await context.Problems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Problem>> GetFiltered(
        string? title,
        string? code,
        string? answer,
        Category? category,
        Difficulty? difficulty,
        DateTime? createdAtFrom,
        DateTime? createdAtTo,
        CancellationToken cancellationToken)
    {
        var query = context.Problems.AsQueryable();
        
        if (!String.IsNullOrEmpty(title))
        {
            query = query.Where(x => x.Title.Contains(title));
        }
        if (!String.IsNullOrEmpty(code))
        {
            query = query.Where(x => x.Code.Contains(code));
        }
        if (!String.IsNullOrEmpty(answer))
        {
            query = query.Where(x => x.Answer.Contains(answer));
        }
        if (category != null)
        {
            query = query.Where(x => x.Category == category);
        }
        if (difficulty != null)
        {
            query = query.Where(x => x.Difficulty == difficulty);
        }
        if (createdAtFrom.HasValue)
        {
            query = query.Where(x => x.CreatedAt >= createdAtFrom);
        }
        if (createdAtTo.HasValue)
        {
            query = query.Where(x => x.CreatedAt <= createdAtTo);
        }
        
        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }
    
    public async Task<Option<Problem>> GetByTitle(string title, CancellationToken cancellationToken)
    {
        return await context.Problems.FirstOrDefaultAsync(x => x.Title == title, cancellationToken);
    }

    public async Task<Problem> Add(Problem problem, CancellationToken cancellationToken)
    {
        await context.Problems.AddAsync(problem, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return problem;
    }

    public async Task<Problem> Update(Problem problem, CancellationToken cancellationToken)
    {
        context.Problems.Update(problem);
        await context.SaveChangesAsync(cancellationToken);
        return problem;
    }

    public async Task Delete(Problem problem, CancellationToken cancellationToken)
    {
        context.Problems.Remove(problem);
        await context.SaveChangesAsync(cancellationToken);
    }
}