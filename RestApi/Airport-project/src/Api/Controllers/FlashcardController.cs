using Api.Dtos;
using Application.Flashcards.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("flashcards")]
[ApiController]

public class FlashcardController(IFlashcardService flashcardService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FlashcardDto>>> GetFlashcards(CancellationToken cancellationToken)
    {
        var flights = await flashcardService.GetFlashcards(cancellationToken);
        return flights.Select(f => FlashcardDto.FromDomainModel(f)).ToList();
    }

    [HttpGet("{flashcardId:guid}")]
    public async Task<ActionResult<FlashcardDto>> GetFlashcard(Guid flashcardId, CancellationToken cancellationToken)
    {
        var flashcard = await flashcardService.GetFlashcard(flashcardId, cancellationToken);
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
        try
        {
            var flashcard = await flashcardService.Add(
                request.Question,
                request.Hint,
                request.Answer,
                request.Description,
                cancellationToken);

            var flashcardDto = FlashcardDto.FromDomainModel(flashcard);
            return CreatedAtAction(nameof(GetFlashcard), new { flashcardId = flashcard.Id }, flashcardDto);
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
        try
        {
            var flashcard = await flashcardService.Update(
                flashcardId,
                request.Question,
                request.Hint,
                request.Answer,
                request.Description,
                request.Score,
                cancellationToken);
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
        var isDeleted = await flashcardService.Delete(flashcardId, cancellationToken);
        if (isDeleted)
        {
            return NoContent();
        }
        return NotFound();
    }
}