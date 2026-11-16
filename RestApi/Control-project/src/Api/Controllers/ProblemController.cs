using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Problems.Commands;
using Domain.Problems;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[Route("problems")]
[ApiController]
public class ProblemController(ISender sender, IProblemQueries problemQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProblemDto>>> GetProblems(CancellationToken cancellationToken)
    {
        var problems = await problemQueries.GetAll(cancellationToken);
        return problems.Select(ProblemDto.FromDomainModel).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProblemDto>> GetProblem(Guid id, CancellationToken cancellationToken)
    {
        var problem = await problemQueries.GetById(new ProblemId(id), cancellationToken);
        return problem.Match<ActionResult<ProblemDto>>(
            p => ProblemDto.FromDomainModel(p),
            () => NotFound());
    }

    [HttpPost("filter")]
    public async Task<ActionResult<IReadOnlyList<ProblemDto>>> GetFiltered(
        [FromBody] SearchProblemDto request,
        CancellationToken cancellationToken)
    {
        var filteredProblems = await problemQueries.GetFiltered(
            title: request.Title,
            code: request.Code,
            answer: request.Answer,
            category: request.Category,
            difficulty: request.Difficulty,
            createdAtFrom: request.CreatedAtFrom,
            createdAtTo: request.CreatedAtTo,
            cancellationToken);

        return filteredProblems.Select(ProblemDto.FromDomainModel).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<ProblemDto>> CreateProblem(
        [FromBody] CreateProblemDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateProblemCommand
        {
            Title = request.Title,
            Code = request.Code,
            Category = request.Category,
            Difficulty = request.Difficulty,
            Answer = request.Answer
        };

        var problem = await sender.Send(input, cancellationToken);

        return problem.Match<ActionResult<ProblemDto>>(
            p => CreatedAtAction(nameof(GetProblem), new { id = p.Id.Value }, ProblemDto.FromDomainModel(p)),
            e => e.ToObjectResult());
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProblemDto>> UpdateProblem(
        Guid id,
        [FromBody] UpdateProblemDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateProblemCommand
        {
            ProblemId = id,
            Title = request.Title,
            Code = request.Code,
            Category = request.Category,
            Difficulty = request.Difficulty,
            Answer = request.Answer
        };
        
        var problem = await sender.Send(input, cancellationToken);
        
        return problem.Match<ActionResult<ProblemDto>>(
            f => Ok(ProblemDto.FromDomainModel(f)),
            e => e.ToObjectResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteProblem(Guid id, CancellationToken cancellationToken)
    {
        var input = new DeleteProblemCommand
        {
            ProblemId = id
        };

        var problem = await sender.Send(input, cancellationToken);

        return problem.Match<ActionResult>(
            _ => NoContent(),
            e => e.ToObjectResult());
    }
}