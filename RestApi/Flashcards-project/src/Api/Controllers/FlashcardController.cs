using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Flashcards.Commands;
using Domain.Flashcards;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("flashcards")]
[ApiController]

public class FlashcardController(ISender sender, IFlashcardQueries flashcardQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FlashcardDto>>> GetFlashcards(CancellationToken cancellationToken)
    {
        var flashcards = await flashcardQueries.GetAll(cancellationToken);
        return flashcards.Select(FlashcardDto.FromDomainModel).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FlashcardDto>> GetFlashcard(Guid id, CancellationToken cancellationToken)
    {
        var flashcard = await flashcardQueries.GetById(new FlashcardId(id), cancellationToken);
        return flashcard.Match<ActionResult<FlashcardDto>>(
            f => FlashcardDto.FromDomainModel(f),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<FlashcardDto>> CreateFlashcard(
        [FromBody] CreateFlashcardDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateFlashcardCommand
        {
            Question = request.Question,
            Hint = request.Hint,
            Answer = request.Answer,
            Description = request.Description
        };
        
        var flashcard = await sender.Send(input, cancellationToken);
        return flashcard.Match<ActionResult<FlashcardDto>>(
            f => CreatedAtAction(nameof(GetFlashcard), new { id = f.Id.Value }, FlashcardDto.FromDomainModel(f)),
            e => e.ToObjectResult());
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<FlashcardDto>> UpdateFlashcard(
        Guid id,
        [FromBody] UpdateFlashcardDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateFlashcardCommand
        {
            FlashcardId = id,
            Question = request.Question,
            Hint = request.Hint,
            Answer = request.Answer,
            Description = request.Description,
            Score = request.Score,
            DueDate = request.DueDate
        };
        
        var flashcard = await sender.Send(input, cancellationToken);
        
        return flashcard.Match<ActionResult<FlashcardDto>>(
            f => Ok(FlashcardDto.FromDomainModel(f)),
            e => e.ToObjectResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteFlashcard(Guid id, CancellationToken cancellationToken)
    {
        var input = new DeleteFlashcardCommand
        {
            FlashcardId = id
        };
        
        var flashcard = await sender.Send(input, cancellationToken);
        return flashcard.Match<ActionResult>(
            f => NoContent(),
            e => e.ToObjectResult());
    }
}