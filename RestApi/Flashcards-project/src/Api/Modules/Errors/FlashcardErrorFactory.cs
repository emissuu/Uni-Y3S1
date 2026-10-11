using Application.Flashcards.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class FlashcardErrorFactory
{
    public static ObjectResult ToObjectResult(this FlashcardException error)
    {
        return new ObjectResult(error.Message)
        {
            StatusCode = error switch
            {
                FlashcardAlreadyExistsException => StatusCodes.Status409Conflict,
                FlashcardNotFoundException => StatusCodes.Status404NotFound,
                FlashcardUnhandledException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Flashcard handler is not implemented.")
            }
        };
    }
}