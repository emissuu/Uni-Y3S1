using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Flashcards.Commands;
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

    [HttpGet("{flashcardId:guid}")]
    public async Task<ActionResult<FlashcardDto>> GetFlashcard(Guid flashcardId, CancellationToken cancellationToken)
    {
        var flashcard = await flashcardQueries.GetById(flashcardId, cancellationToken);
        if (flashcard is null)
        {
            return NotFound();
        }

        return FlashcardDto.FromDomainModel(flashcard);
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

        try
        {
            var flashcard = await sender.Send(input, cancellationToken);
            var dto = FlashcardDto.FromDomainModel(flashcard);

            return CreatedAtAction(nameof(GetFlashcard), new { flashcardId = dto.Id }, dto);
        }
        catch (ArgumentException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpPut("{flashcardId:guid}")]
    public async Task<ActionResult<FlashcardDto>> UpdateFlashcard(
        Guid flashcardId,
        [FromBody] UpdateFlashcardDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateFlashcardCommand
        {
            FlashcardId = flashcardId,
            Question = request.Question,
            Hint = request.Hint,
            Answer = request.Answer,
            Description = request.Description,
            Score = request.Score,
            DueDate = request.DueDate
        };
        
        try
        {
            var flashcard = await sender.Send(input, cancellationToken);
            if (flashcard is null)
            {
                return NotFound($"Flashcard with id {flashcardId} not found");
            }
            
            var flashcardDto = FlashcardDto.FromDomainModel(flashcard);
            return Ok(flashcardDto);
        }
        catch (ArgumentException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpDelete("{flashcardId:guid}")]
    public async Task<ActionResult> DeleteFlashcard(Guid flashcardId, CancellationToken cancellationToken)
    {
        var input = new DeleteFlashcardCommand
        {
            FlashcardId = flashcardId
        };
        
        var isDeleted = await sender.Send(input, cancellationToken);
        if (isDeleted)
        {
            return NoContent();
        }
        return NotFound();
    }
}